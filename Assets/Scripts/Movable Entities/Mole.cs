using UnityEngine;
[RequireComponent(typeof(SpriteRenderer))]
public class Mole : MovableEntity
{
    public enum Genders { Male, Female };

    [Space(10, order = 0)]
    [Header("Mole Properties", order = 1)]
    public Genders gender;
    public Color surfaceColor;
    public Color undergroundColor;
    private float activityValue;
    private bool isUnderGround;
    private SpriteRenderer spriteRenderer;
    private int movesLeft;
    private float timeWhenNextAction;
    private float timeWhenNextMove;
    private float timeWhenNextPopulationAllowed;

    public bool IsFemale { get; private set; }

    public float GetTimeWhenNextPopulationAllowed()
    {
        return timeWhenNextPopulationAllowed;
    }

    public bool IsUnderGround
    {
        get
        {
            return isUnderGround;
        }
        private set
        {
            isUnderGround = value;
            spriteRenderer.flipX = isUnderGround;
            spriteRenderer.flipY = isUnderGround;
            if (isUnderGround)
            {
                spriteRenderer.color = undergroundColor;
                spriteRenderer.sortingOrder = -1;
            }
            else
            {
                spriteRenderer.color = surfaceColor;
                spriteRenderer.sortingOrder = 1;
            }
        }
    }

    private void ChangeFloor(bool _isUnderGround)
    {
        IsUnderGround = _isUnderGround;
    }

    private void SetGender()
    {
        switch (gender)
        {
            case Genders.Male:
                {
                    IsFemale = false;
                    break;
                }
            case Genders.Female:
                {
                    IsFemale = true;
                    break;
                }
        }
    }

    private void SetAction()
    {
        float randomValue = Random.value;
        if (randomValue < 0.1f)
        {
            Wait(Random.Range(3f, 5f));
        }
        else if (randomValue < 0.6f * activityValue + 0.25f)
        {
            SetDestinationPoint(GameField.instance.GetRandomPointInGameField());
        }
        else
        {
            GoToSurfaceAndEat(Random.Range(3f, 5f));
        }
    }

    private void Wait(float waitingTime)
    {
        timeWhenNextAction = GameController.instance.sessionTime + waitingTime;
    }

    private void GoToSurfaceAndEat(float waitingTime)
    {
        movesLeft = Mathf.FloorToInt(waitingTime);
        ChangeFloor(false);
        Wait(waitingTime);
        timeWhenNextMove = GameController.instance.sessionTime + 1f;
    }

    protected override void DestinationPointReached()
    {
        transform.position = destinationPoint;
        IsMoving = false;
        SetAction();
    }

    private void MakeMoveOnSurface()
    {
        GameController.instance.EatVegetable();
        movesLeft--;
        timeWhenNextMove += 1f;
    }

    private void GiveBirth()
    {
        MoleController.instance.SpawnMole(transform.position);
        movesLeft--;
    }

    private void ReturnToUnderground()
    {
        ChangeFloor(true);
        SetAction();
    }

    private void SetNextPopulationTime()
    {
        timeWhenNextPopulationAllowed = GameController.instance.sessionTime + 10f;
    }

    public void IncreasePopulation()
    {
        IsMoving = false;
        SetNextPopulationTime();
        if (IsFemale)
        {
            movesLeft = 1;
            timeWhenNextMove = GameController.instance.sessionTime + 1f;
        }
        Wait(2f);
    }

    protected override void Awake()
    {
        base.Awake();
        spriteRenderer = GetComponent<SpriteRenderer>();
        SetGender();
        SetNextPopulationTime();
        activityValue = Random.value;
        ChangeFloor(true);
    }

    private void Start()
    {
        MoleController.instance.AddToMolesList(this);
        SetAction();
    }

    protected override void Update()
    {
        if (isUnderGround)
        {
            if (IsMoving)
            {
                MoveEntity();
            }
            else
            {
                float sessionTime = GameController.instance.sessionTime;
                if (sessionTime < timeWhenNextAction)
                {
                    if (movesLeft > 0 && sessionTime >= timeWhenNextMove) GiveBirth();
                }
                else
                {
                    SetAction();
                }
            }
        }
        else
        {
            float sessionTime = GameController.instance.sessionTime;
            if (sessionTime < timeWhenNextAction)
            {
                if (movesLeft > 0 && sessionTime >= timeWhenNextMove) MakeMoveOnSurface();
            }
            else
            {
                ReturnToUnderground();
            }
        }
    }

    private void OnDestroy()
    {
        MoleController.instance.RemoveFromMolesList(this);
    }
}