using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SaveLoadItem : MonoBehaviour
{
    private string mapName;

    public SaveLoadMenu menu;

    public string MapName
    {
        get => mapName;
        set
        {
            mapName = value;
            transform.GetChild(0).GetComponent<Text>().text = value;
        }
    }

    public void Select()
    {
        menu.SelectItem(mapName);
    }
}