using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HexCellShaderData : MonoBehaviour
{
    private Texture2D cellTexture;
    private Color32[] cellTextureData;

    private void LateUpdate()
    {
        cellTexture.SetPixels32(cellTextureData);
        cellTexture.Apply();
        enabled = false;
    }

    public void Initialize(int x, int z)
    {
        if (cellTexture) cellTexture.Reinitialize(x, z);
        else
        {
            cellTexture = new(x, z, TextureFormat.RGBA32, false, true)
            {
                filterMode = FilterMode.Point,
                wrapMode = TextureWrapMode.Clamp
            };
            Shader.SetGlobalTexture("_HexCellData", cellTexture);
        }
        Shader.SetGlobalVector("_HexCellData_TexelSize", new(1f / x, 1f / z, x, z));

        if (cellTextureData == null || cellTextureData.Length != x * z)
            cellTextureData = new Color32[x * z];
        else
        {
            for (int i = 0; i < cellTextureData.Length; i++)
                cellTextureData[i] = new(0, 0, 0, 0);
        }

        enabled = true;
    }

    public void RefreshTerrain(HexCell cell)
    {
        cellTextureData[cell.Index].a = (byte)cell.TerrainTypeIndex;
        enabled = true;
    }

    public void RefreshVisibility(HexCell cell)
    {
        int index = cell.Index;
        cellTextureData[index].r = cell.IsVisible ? (byte)255 : (byte)0;
        cellTextureData[index].g = cell.IsExplored ? (byte)255 : (byte)0;
        enabled = true;
    }
}