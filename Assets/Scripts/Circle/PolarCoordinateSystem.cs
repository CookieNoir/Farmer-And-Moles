using UnityEngine;

public static class PolarCoordinateSystem
{
    public const float DualPI = Mathf.PI * 2f;

    public static Vector3 GetPosition(float angleRadians, float radius)
    {
        return new Vector3(radius * Mathf.Cos(angleRadians), radius * Mathf.Sin(angleRadians));
    }
}