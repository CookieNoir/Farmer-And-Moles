using System;
using UnityEngine;
[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class Circle: MonoBehaviour
{
    public float radius;
    [Min(0.02f)] public float thickness;
    private MeshFilter meshFilter;
    private Vector3[] pointsPositions;
    private Mesh mesh;

    private void Start()
    {
        PrepareMesh();
    }

    private void PrepareMesh()
    {
        meshFilter = GetComponent<MeshFilter>();
        pointsPositions = new Vector3[UniversalCircle.instance.PointsCountWithSecondCircle];
        mesh = new Mesh();
        GetCircle();
        mesh.uv = UniversalCircle.instance.uvs;
        mesh.triangles = UniversalCircle.instance.triangles;
        meshFilter.mesh = mesh;
    }

    public void GetCircle()
    {
        Array.Copy(UniversalCircle.instance.pointsUniversalPositions, pointsPositions, UniversalCircle.instance.PointsCountWithSecondCircle);
        for (int i = UniversalCircle.instance.PointsCount, j = UniversalCircle.instance.PointsCountWithSecondCircle - 1; i > -1; --i, --j)
        {
            pointsPositions[i] *= radius - thickness;
            pointsPositions[j] *= radius + thickness;
        }
        mesh.vertices = pointsPositions;
    }
}