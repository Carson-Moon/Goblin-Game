#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(MarkerPrefabBaker))]
public class MarkerPrefabBakerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        var baker = (MarkerPrefabBaker)target;

        EditorGUILayout.Space();

        if (GUILayout.Button("Bake Prefabs From Markers"))
        {
            Bake(baker);
        }

        if (GUILayout.Button("Select Baked Instances"))
        {
            SelectBaked(baker);
        }
    }

    private void Bake(MarkerPrefabBaker baker)
    {
        if (baker == null) return;

        // register the undo for the scene / prefab
        Undo.RegisterFullObjectHierarchyUndo(baker.gameObject, "Bake Prefabs From Markers");

        baker.BakeMarkers();

        // Mark the scene/prefab dirty so changes are saved
        EditorUtility.SetDirty(baker.gameObject);
        if (!Application.isPlaying)
        {
            var scene = baker.gameObject.scene;
            if (scene.IsValid())
            {
                UnityEditor.SceneManagement.EditorSceneManager.MarkSceneDirty(scene);
            }
        }
    }

    private void SelectBaked(MarkerPrefabBaker baker)
    {
        if (baker == null || baker.bakedInstances == null || baker.bakedInstances.Count == 0)
        {
            Debug.Log("[MarkerPrefabBakerEditor] No baked instances to select.");
            return;
        }

        Selection.objects = baker.bakedInstances.ToArray();
    }
}
#endif