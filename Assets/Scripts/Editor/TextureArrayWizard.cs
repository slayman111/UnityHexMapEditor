using UnityEditor;
using UnityEngine;

public class TextureArrayWizard : ScriptableWizard
{
    public Texture2D[] textures;

    void OnWizardCreate()
    {
        if (textures.Length == 0) return;

        string path = EditorUtility.SaveFilePanelInProject("Save Texture Array", "Texture Array", "asset", "Save Texture Array");
        if (path.Length == 0) return;

        Texture2D t = textures[0];
        Texture2DArray textureArray = new(t.width, t.height, textures.Length, t.format, t.mipmapCount > 1)
        {
            anisoLevel = t.anisoLevel,
            filterMode = t.filterMode,
            wrapMode = t.wrapMode
        };

        for (int i = 0; i < textures.Length; i++)
            for (int m = 0; m < t.mipmapCount; m++)
                Graphics.CopyTexture(textures[i], 0, m, textureArray, i, m);

        AssetDatabase.CreateAsset(textureArray, path);
    }

    [MenuItem("Assets/Create/Texture Array")]
    private static void CreateWizard()
    {
        DisplayWizard<TextureArrayWizard>("Create Texture Array", "Create");
    }
}