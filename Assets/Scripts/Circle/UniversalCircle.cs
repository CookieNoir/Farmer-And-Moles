using UnityEngine;

public class UniversalCircle : Singleton<UniversalCircle>
{
    public int PointsCount = 128;
    public float PointsCountInverted = 0.0078125f;
    public int PointsCountWithSecondCircle = 258; // (PointsCount + 1) * 2. Another point is added for fine looking UV unwrap

    [HideInInspector] public Vector3[] pointsUniversalPositions; // Calculated ONCE for the circle with radius = 1 and duplicated for the second circle
    [HideInInspector] public Vector2[] uvs;
    [HideInInspector] public int[] triangles;

    protected override void InitializeFields()
    {
        pointsUniversalPositions = new Vector3[PointsCountWithSecondCircle];
        uvs = new Vector2[PointsCountWithSecondCircle];
        triangles = new int[PointsCount * 6];
        int topIndex = PointsCountWithSecondCircle - 1, p0 = PointsCount, p1 = topIndex, triangleIndex = PointsCount * 6;
        float offset = 1f;
        Vector3 vector3 = new Vector3(1f, 0f);

        pointsUniversalPositions[PointsCount] = vector3;
        uvs[PointsCount] = new Vector2(offset, 0f);

        pointsUniversalPositions[topIndex] = vector3;
        uvs[topIndex] = new Vector2(offset, 1f);

        for (int i = PointsCount - 1; i > -1; --i)
        {
            offset -= PointsCountInverted;
            vector3 = PolarCoordinateSystem.GetPosition(offset * PolarCoordinateSystem.DualPI, 1f);
            pointsUniversalPositions[i] = vector3;
            uvs[i] = new Vector2(offset, 0f);

            triangleIndex -= 6;
            triangles[triangleIndex] = p0;
            triangles[triangleIndex + 1] = p1;
            triangles[triangleIndex + 2] = i;
            p0 = i;

            topIndex--;
            pointsUniversalPositions[topIndex] = vector3;
            uvs[topIndex] = new Vector2(offset, 1f);

            triangles[triangleIndex + 3] = p0;
            triangles[triangleIndex + 4] = p1;
            triangles[triangleIndex + 5] = topIndex;
            p1 = topIndex;
        }
    }
}
