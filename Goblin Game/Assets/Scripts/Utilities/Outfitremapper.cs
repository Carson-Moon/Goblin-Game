using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif
 
namespace ModularClothing
{
    /// <summary>
    /// Editor tool that remaps SkinnedMeshRenderer bones on outfit meshes
    /// (children named with the OUTFIT_ prefix) to bones on a target model's rig.
    ///
    /// Usage:
    ///   1. Add this component to any GameObject in the scene (it's a tool, not runtime logic).
    ///   2. Drag your target/base character prefab or scene instance into "Target Model".
    ///   3. Drag the outfit container (parent of the OUTFIT_ meshes) into "Outfit Source".
    ///   4. Click "Remap Outfits" in the inspector.
    ///
    /// The remapped meshes will have their bones[] re-pointed at the target model's rig,
    /// and their rootBone will be set to the equivalent transform on the target.
    /// Changes are registered with Undo so Ctrl+Z works cleanly.
    /// </summary>
    public class OutfitRemapper : MonoBehaviour
    {
        [Tooltip("The base character/model whose rig the outfits should be bound to.")]
        public GameObject targetModel;
 
        [Tooltip("The container holding the outfit meshes. The tool will search its children " +
                 "for SkinnedMeshRenderers whose GameObject name starts with the outfit prefix.")]
        public GameObject outfitSource;
 
        [Tooltip("Prefix that identifies outfit meshes to remap.")]
        public string outfitPrefix = "OUTFIT_";
 
        [Tooltip("If true, the original armature GameObjects under the outfit source will be " +
                 "disabled (not deleted) after remap, so undo can restore cleanly.")]
        public bool disableOriginalArmature = true;
    }
 
#if UNITY_EDITOR
    [CustomEditor(typeof(OutfitRemapper))]
    public class OutfitRemapperEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();
 
            OutfitRemapper tool = (OutfitRemapper)target;
 
            EditorGUILayout.Space();
 
            using (new EditorGUI.DisabledScope(tool.targetModel == null || tool.outfitSource == null))
            {
                if (GUILayout.Button("Remap Outfits", GUILayout.Height(30)))
                {
                    RemapOutfits(tool);
                }
            }
 
            if (tool.targetModel == null || tool.outfitSource == null)
            {
                EditorGUILayout.HelpBox("Assign both Target Model and Outfit Source to enable remapping.", MessageType.Info);
            }
        }
 
        private void RemapOutfits(OutfitRemapper tool)
        {
            // Build a name -> Transform map of every bone in the target model's hierarchy.
            // We walk all descendants so it doesn't matter how deep the rig is nested.
            Dictionary<string, Transform> targetBoneMap = new Dictionary<string, Transform>();
            Transform[] targetTransforms = tool.targetModel.GetComponentsInChildren<Transform>(true);
            foreach (Transform t in targetTransforms)
            {
                // First occurrence wins. If you have duplicate bone names in the target,
                // that's a separate problem you'd need to resolve in Blender.
                if (!targetBoneMap.ContainsKey(t.name))
                    targetBoneMap.Add(t.name, t);
            }
 
            // Find all SkinnedMeshRenderers under the outfit source whose GameObject
            // name starts with the configured prefix.
            SkinnedMeshRenderer[] allRenderers = tool.outfitSource.GetComponentsInChildren<SkinnedMeshRenderer>(true);
            List<SkinnedMeshRenderer> outfits = new List<SkinnedMeshRenderer>();
            foreach (SkinnedMeshRenderer smr in allRenderers)
            {
                if (smr.gameObject.name.StartsWith(tool.outfitPrefix))
                    outfits.Add(smr);
            }
 
            if (outfits.Count == 0)
            {
                EditorUtility.DisplayDialog("Outfit Remapper",
                    $"No SkinnedMeshRenderers found under '{tool.outfitSource.name}' " +
                    $"with names starting with '{tool.outfitPrefix}'.", "OK");
                return;
            }
 
            int successCount = 0;
            int missingBoneCount = 0;
            List<string> warnings = new List<string>();
 
            // Group all changes under one undo entry so Ctrl+Z reverts the whole operation.
            int undoGroup = Undo.GetCurrentGroup();
            Undo.SetCurrentGroupName("Remap Outfits");
 
            foreach (SkinnedMeshRenderer smr in outfits)
            {
                Undo.RecordObject(smr, "Remap Outfit Bones");
 
                // Remap rootBone by name.
                Transform originalRoot = smr.rootBone;
                if (originalRoot != null)
                {
                    if (targetBoneMap.TryGetValue(originalRoot.name, out Transform newRoot))
                    {
                        smr.rootBone = newRoot;
                    }
                    else
                    {
                        warnings.Add($"[{smr.name}] Could not find root bone '{originalRoot.name}' in target.");
                    }
                }
                else
                {
                    warnings.Add($"[{smr.name}] Source had no rootBone assigned.");
                }
 
                // Remap the bones[] array. The array's order must be preserved because
                // it maps 1:1 with the BoneWeight indices baked into the mesh.
                Transform[] originalBones = smr.bones;
                Transform[] newBones = new Transform[originalBones.Length];
                int localMissing = 0;
 
                for (int i = 0; i < originalBones.Length; i++)
                {
                    Transform original = originalBones[i];
                    if (original == null)
                    {
                        // A null entry in bones[] is unusual but possible; preserve it.
                        newBones[i] = null;
                        continue;
                    }
 
                    if (targetBoneMap.TryGetValue(original.name, out Transform replacement))
                    {
                        newBones[i] = replacement;
                    }
                    else
                    {
                        // No matching bone found. Leaving this null would make the mesh
                        // deform incorrectly, so we keep the original reference and warn.
                        newBones[i] = original;
                        localMissing++;
                    }
                }
 
                smr.bones = newBones;
 
                if (localMissing > 0)
                {
                    warnings.Add($"[{smr.name}] {localMissing} bone(s) had no match in target and were left pointing at the original rig.");
                    missingBoneCount += localMissing;
                }
 
                // Give the renderer generous local bounds so it won't get frustum-culled
                // if the new bones produce unexpected positions during the swap. You can
                // tighten this later or leave updateWhenOffscreen on during development.
                Bounds b = smr.localBounds;
                if (b.size == Vector3.zero)
                {
                    smr.localBounds = new Bounds(Vector3.zero, Vector3.one * 2f);
                }
 
                successCount++;
            }
 
            // Optionally disable the original armature(s) so the scene stays clean
            // but undo can still restore the original state.
            if (tool.disableOriginalArmature)
            {
                // Look for armature roots inside the outfit source. Heuristic: top-level
                // children of outfitSource that contain bones referenced by any outfit's
                // original setup. Simpler approach: disable any child that isn't an outfit mesh.
                foreach (Transform child in tool.outfitSource.transform)
                {
                    bool isOutfitMesh = child.name.StartsWith(tool.outfitPrefix) &&
                                        child.GetComponent<SkinnedMeshRenderer>() != null;
                    if (!isOutfitMesh && child.gameObject.activeSelf)
                    {
                        Undo.RecordObject(child.gameObject, "Disable Original Armature");
                        child.gameObject.SetActive(false);
                    }
                }
            }
 
            Undo.CollapseUndoOperations(undoGroup);
 
            // Mark the scene dirty so changes persist on save.
            if (!Application.isPlaying)
            {
                UnityEditor.SceneManagement.EditorSceneManager.MarkSceneDirty(
                    UnityEditor.SceneManagement.EditorSceneManager.GetActiveScene());
            }
 
            string summary = $"Remapped {successCount} outfit mesh(es).";
            if (missingBoneCount > 0)
                summary += $"\n{missingBoneCount} bone(s) had no match in the target rig.";
            if (warnings.Count > 0)
                summary += "\n\nWarnings:\n - " + string.Join("\n - ", warnings);
 
            Debug.Log("[OutfitRemapper] " + summary);
            EditorUtility.DisplayDialog("Outfit Remapper", summary, "OK");
        }
    }
#endif
}