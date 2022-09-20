using UnityEngine;

public class Bezier : MonoBehaviour
{
    public static Vector3 GetPoint(Vector3 a, Vector3 b, Vector3 c, float t)
    {
        float r = 1f - t;
        return r * r * a + 2f * r * t * b + t * t * c;
    }

    public static Vector3 GetDerivative(Vector3 a, Vector3 b, Vector3 c, float t) =>
        2f * ((1f - t) * (b - a) + t * (c - b));
}