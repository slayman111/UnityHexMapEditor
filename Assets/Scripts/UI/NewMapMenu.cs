using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewMapMenu : MonoBehaviour
{
    private bool generateMaps = true;

    public HexGrid hexGrid;

    public HexMapGenerator mapGenerator;

    private bool wrapping = true;

    public void Open()
    {
        gameObject.SetActive(true);
        HexMapCamera.Locked = true;
    }

    public void Close()
    {
        gameObject.SetActive(false);
        HexMapCamera.Locked = false;
    }

    public void ToggleMapGeneration(bool toggle) => generateMaps = toggle;

    public void ToggleWrapping(bool toggle) => wrapping = toggle;

    public void CreateSmallMap() => CreateMap(20, 15);

    public void CreateMediumMap() => CreateMap(40, 30);

    public void CreateLargeMap() => CreateMap(80, 60);

    private void CreateMap(int x, int z)
    {
        if (generateMaps) mapGenerator.GenerateMap(x, z, wrapping);
        else hexGrid.CreateMap(x, z, wrapping);
        HexMapCamera.ValidatePosition();
        Close();
    }
}