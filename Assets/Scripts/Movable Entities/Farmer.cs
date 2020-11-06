using UnityEngine;

public class Farmer : MovableEntity
{
    [Space(10, order = 0)]
    [Header("Farmer Properties", order = 1)]
    [Min(0f)] public float attackRadius;
    public Circle circle;
    private Mole target;

    private void Start()
    {
        circle.radius = attackRadius;
    }

    public void OnStart()
    {
        transform.position = Vector3.zero;
        SetTarget();
    }

    protected override void MoveEntity()
    {
        float step = speed * Time.deltaTime;
        if (IsDestinationPointReached(attackRadius)) DestinationPointReached();
        else MakeStep(step);
    }

    protected override void DestinationPointReached()
    {
        IsMoving = false;
        MoleController.instance.DestroyAllMolesOnSurfaceInRadius(transform.position, attackRadius);
        target = null;
    }

    protected void SetTarget()
    {
        IsMoving = false;
        target = MoleController.instance.GetNearestMoleOnSurface(transform.position);
        if (target) SetDestinationPoint(target.transform.position);
    }

    private void OnValidate()
    {
        circle.radius = attackRadius;
    }

    protected override void Update()
    {
        if (IsMoving && !target.IsUnderGround)
        {
            MoveEntity();
        }
        else
        {
            SetTarget();
        }
    }
}