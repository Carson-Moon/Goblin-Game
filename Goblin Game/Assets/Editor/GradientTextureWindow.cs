using UnityEditor;
using UnityEngine;

public class GradientTextureWindow : EditorWindow
{
    private Material targetMaterial;
    private Gradient gradient = new Gradient();

    [MenuItem("Tools/Gradient Texture Maker")]
    public static void Open()
    {
        GetWindow<GradientTextureWindow>("Gradient Texture Maker");
    }

    private void OnGUI()
    {
        EditorGUILayout.LabelField("Gradient Texture Generator", EditorStyles.boldLabel);
        EditorGUILayout.Space();

        targetMaterial = (Material)EditorGUILayout.ObjectField(
            "Target Material",
            targetMaterial,
            typeof(Material),
            false
        );

        gradient = EditorGUILayout.GradientField("Gradient", gradient);

        EditorGUILayout.Space();

        EditorGUI.BeginDisabledGroup(targetMaterial == null);
        if (GUILayout.Button("Generate Gradient Texture"))
        {
            Generate();
        }
        EditorGUI.EndDisabledGroup();
    }

    private void Generate()
    {
        Texture2D tex = GradientTextureMaker.CreateGradientTexture(targetMaterial, gradient);
        if (targetMaterial.HasProperty("_GradientTex"))
        {
            targetMaterial.SetTexture("_GradientTex", tex);
        }

        EditorUtility.SetDirty(targetMaterial);
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
    }
}