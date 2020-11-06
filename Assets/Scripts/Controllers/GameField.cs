using UnityEngine;
using Random = UnityEngine.Random;

public class GameField : Singleton<GameField>
{
    public Circle circle;
    public float radius;

    protected override void InitializeFields()
    {
        circle.radius = radius;
    }

    public Vector3 GetRandomPointInGameField()
    {
        float polarRadius = radius * Mathf.Sqrt(Random.value);
        float angle = Random.value * PolarCoordinateSystem.DualPI;
        return PolarCoordinateSystem.GetPosition(angle, polarRadius);
    }

    private void OnValidate()
    {
        circle.radius = radius;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, radius);
    }
}