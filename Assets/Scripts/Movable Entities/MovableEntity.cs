using UnityEngine;

public class MovableEntity : MonoBehaviour
{
    [Space(10, order = 0)]
    [Header("Movable Entity Properties", order = 1)]
    public float startSpeed = 1f;
    protected float speed;
    protected Vector3 destinationPoint;

    public bool IsMoving { get; protected set; }

    protected virtual void Awake()
    {
        speed = startSpeed;
        IsMoving = false;
    }

    public void SetDestinationPoint(Vector3 newDestinationPoint)
    {
        destinationPoint = newDestinationPoint;
        IsMoving = true;
    }

    protected bool IsDestinationPointReached(float step)
    {
        return Vector3.Distance(transform.position, destinationPoint) <= step;
    }

    protected void MakeStep(float step)
    {
        transform.position = Vector3.MoveTowards(transform.position, destinationPoint, step);
    }

    protected virtual void DestinationPointReached()
    {
        transform.position = destinationPoint;
        IsMoving = false;
    }

    protected virtual void MoveEntity()
    {
        float step = speed * Time.deltaTime;
        if (IsDestinationPointReached(step)) DestinationPointReached();
        else MakeStep(step);
    }

    protected virtual void Update()
    {
        if (IsMoving)
        {
            MoveEntity();
        }
    }
}