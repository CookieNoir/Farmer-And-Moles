using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class MoleController : Singleton<MoleController>
{
    [Tooltip("X - females, Y - males")]
    public Vector2Int molesCountAtStart;
    public float molesIncreasePopulationRadius = 0.5f;
    [Space(10)]
    public GameObject moleMalePrefab;
    public GameObject moleFemalePrefab;
    public event Action<int, int> updateMolesCount;

    private int molesMaleCount;
    private int molesFemaleCount;
    private int molesTotalCount;
    private List<Mole> moles;

    protected override void InitializeFields()
    {
        moles = new List<Mole>();
        molesMaleCount = 0;
        molesFemaleCount = 0;
        molesTotalCount = 0;
    }

    public void SpawnMoles()
    {
        foreach (Mole mole in moles)
        {
            if (mole) Destroy(mole.gameObject);
        }
        moles.Clear();
        for (int i = 0; i < molesCountAtStart.x; ++i)
        {
            Instantiate(moleFemalePrefab, GameField.instance.GetRandomPointInGameField(), Quaternion.identity).GetComponent<Mole>();
        }
        molesFemaleCount = molesCountAtStart.x;
        for (int i = 0; i < molesCountAtStart.y; ++i)
        {
            Instantiate(moleMalePrefab, GameField.instance.GetRandomPointInGameField(), Quaternion.identity).GetComponent<Mole>();
        }
        molesMaleCount = molesCountAtStart.y;
        molesTotalCount = molesFemaleCount + molesMaleCount;
        UpdateMolesCount();
    }

    public void SpawnMole(Vector3 position)
    {
        float value = Random.value;
        if (value > 0.5f)
        {
            Instantiate(moleFemalePrefab, position, Quaternion.identity).GetComponent<Mole>();
            molesFemaleCount++;
        }
        else
        {
            Instantiate(moleMalePrefab, position, Quaternion.identity).GetComponent<Mole>();
            molesMaleCount++;
        }
        molesTotalCount++;
        UpdateMolesCount();
    }

    public Mole GetNearestMoleOnSurface(Vector3 position)
    {
        float distance = float.PositiveInfinity, currentDistance;
        Mole result = null;
        for (int i = 0; i < moles.Count; ++i)
        {
            if (!moles[i].IsUnderGround)
            {
                currentDistance = Vector3.Distance(position, moles[i].transform.position);
                if (currentDistance < distance)
                {
                    result = moles[i];
                    distance = currentDistance;
                }
            }
        }
        return result;
    }

    public void DestroyAllMolesOnSurfaceInRadius(Vector3 position, float radius)
    {
        for (int i = 0; i < moles.Count; ++i)
        {
            if (!moles[i].IsUnderGround)
            {
                if (Vector3.Distance(position, moles[i].transform.position) < radius)
                {
                    if (moles[i].IsFemale) molesFemaleCount--;
                    else molesMaleCount--;
                    molesTotalCount--;
                    UpdateMolesCount();
                    Destroy(moles[i].gameObject);
                }
            }
        }
    }

    private void CheckForPopulationIncrease()
    {
        float time = GameController.instance.sessionTime;
        for (int i = 0; i < moles.Count; ++i)
        {
            if (moles[i].IsUnderGround && time >= moles[i].GetTimeWhenNextPopulationAllowed())
            {
                for (int j = i + 1; j < moles.Count; ++j)
                {
                    if (moles[j].IsUnderGround &&
                        time >= moles[j].GetTimeWhenNextPopulationAllowed() &&
                        Vector3.Distance(moles[i].transform.position, moles[j].transform.position) < molesIncreasePopulationRadius &&
                        moles[i].IsFemale != moles[j].IsFemale)
                    {
                        moles[i].IncreasePopulation();
                        moles[j].IncreasePopulation();
                    }
                }
            }
        }
    }

    public void AddToMolesList(Mole mole)
    {
        moles.Add(mole);
    }

    public void RemoveFromMolesList(Mole mole)
    {
        moles.Remove(mole);
    }

    private void Update()
    {
        CheckForPopulationIncrease();
    }

    private void UpdateMolesCount()
    {
        GameController.instance.MolesCountChanged(molesTotalCount);
        updateMolesCount?.Invoke(molesMaleCount, molesFemaleCount);
    }
}