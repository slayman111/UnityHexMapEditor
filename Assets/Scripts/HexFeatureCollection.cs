using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct HexFeatureCollection
{
    public Transform[] prefabs;

    public Transform Pick(float choice) => prefabs[(int)(choice * prefabs.Length)];
}