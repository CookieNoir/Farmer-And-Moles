using System;
using UnityEngine;

public class GameController : Singleton<GameController>
{
    public Farmer farmer;
    public int vegetablesAtStart;
    public event Action<int> updateVegetablesCount;
    public event Action<bool> updateGameSessionStatus;
    public event Action<string> updateMessage;

    [HideInInspector] public float sessionTime;
    private int vegetablesLeft;
    private bool isPlaying = true;
    private bool enoughMoles = false;

    protected override void InitializeFields()
    {
        vegetablesLeft = 0;
        isPlaying = false;
        enoughMoles = false;
    }

    public void StartGame()
    {
        sessionTime = 0f;
        MoleController.instance.SpawnMoles();
        farmer.OnStart();
        vegetablesLeft = vegetablesAtStart;
        updateVegetablesCount?.Invoke(vegetablesLeft);
        if (enoughMoles) SetGameSessionStatus(true);
    }

    public void EatVegetable()
    {
        vegetablesLeft--;
        updateVegetablesCount?.Invoke(vegetablesLeft);
        if (vegetablesLeft <= 0)
        {
            vegetablesLeft = 0;
            updateMessage?.Invoke("Все овощи съедены. Кроты победили");
            SetGameSessionStatus(false);
        }
    }

    public void MolesCountChanged(int molesLeft)
    {
        if (molesLeft > 0)
        {
            enoughMoles = true;
        }
        else
        {
            enoughMoles = false;
            updateMessage?.Invoke("Все кроты убиты. Фермер победил");
            SetGameSessionStatus(false);
        }
    }

    private void SetGameSessionStatus(bool value)
    {
        isPlaying = value;
        updateGameSessionStatus?.Invoke(isPlaying);
        if (isPlaying)
        {
            Time.timeScale = 1f;
            updateMessage?.Invoke("Идет игровая сессия");
        }
        else Time.timeScale = 0f;
    }

    private void Update()
    {
        if (isPlaying)
        {
            sessionTime += Time.deltaTime;
        }
        if (Input.GetKeyDown(KeyCode.Space))
        {
            StartGame();
        }
    }
}