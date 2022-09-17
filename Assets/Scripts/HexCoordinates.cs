﻿using UnityEngine;

[System.Serializable]
public struct HexCoordinates
{
    [SerializeField]
    private int x, z;

    public int X { get => x; }
    public int Z { get => z; }
    public int Y { get => -X - Z; }

    public HexCoordinates(int x, int z)
    {
        this.x = x;
        this.z = z;
    }

    public static HexCoordinates FromOffsetCoordinates(int x, int z) => new(x - z / 2, z);

    public static HexCoordinates FromPosition(Vector3 position)
    {
        float x = position.x / (HexMetrics.innerRadius * 2f);
        float y = -x;

        float offset = position.z / (HexMetrics.outerRadius * 3f);
        x -= offset;
        y -= offset;

        int iX = Mathf.RoundToInt(x);
        int iY = Mathf.RoundToInt(y);
        int iZ = Mathf.RoundToInt(-x - y);

        if (iX + iY + iZ != 0)
        {
            float dX = Mathf.Abs(x - iX);
            float dY = Mathf.Abs(y - iY);
            float dZ = Mathf.Abs(-x - y - iZ);

            if (dX > dY && dX > dZ) iX = -iY - iZ;
            else if (dZ > dY) iZ = -iX - iY;
        }

        return new(iX, iZ);
    }

    public int DistanceTo(HexCoordinates other) =>
        ((x < other.x ? other.x - x : x - other.x) +
        (Y < other.Y ? other.Y - Y : Y - other.Y) +
        (z < other.z ? other.z - z : z - other.z)) / 2;

    public override string ToString() => "(" + X.ToString() + ", " + Y.ToString() + ", " + Z.ToString() + ")";

    public string ToStringOnSeparateLines() => X.ToString() + "\n" + Y.ToString() + "\n" + Z.ToString();
}