using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    [SerializeField] private int maxMiniBallCount;

    private int activeMiniBallCount;
    private int remainingCubeCount;
    private int remainingQueueBallCount;
    private bool isGameOver;
    public bool IsGameOver => isGameOver;
    public int MaxCapacity => maxMiniBallCount;
    public event Action<int> OnActiveMiniBallCountChanged;

    public event Action OnWin;
    public event Action OnLose;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }

    public void SetTotalCubeCount(int count)
    {
        remainingCubeCount = count;
    }

    public void SetTotalQueueBallCount(int count)
    {
        remainingQueueBallCount = count;
    }

    public void IncreaseMiniBallCount(int countToIncrease)
    {
        activeMiniBallCount += countToIncrease;
        OnActiveMiniBallCountChanged?.Invoke(activeMiniBallCount);
        CheckFail();
    }

    public void DecreaseMiniBallCount()
    {
        activeMiniBallCount--;
        OnActiveMiniBallCountChanged?.Invoke(activeMiniBallCount);
        CheckWin();
    }

    public void DecreaseCubeCount()
    {
        remainingCubeCount--;
        CheckWin();
    }

    public void DecreaseQueueBallCount()
    {
        remainingQueueBallCount--;
        CheckWin();
    }

    private void CheckFail()
    {
        if (isGameOver) return;
        if (activeMiniBallCount > maxMiniBallCount)
            Lose();
    }

    private void CheckWin()
    {
        if (isGameOver) return;
        if (remainingCubeCount == 0 &&
            remainingQueueBallCount == 0 &&
            activeMiniBallCount == 0)
        {
            Win();
        }
    }

    private void Lose()
    {
        if (isGameOver) return;
        isGameOver = true;
        OnLose?.Invoke();
    }

    private void Win()
    {
        if (isGameOver) return;
        isGameOver = true;
        OnWin?.Invoke();
    }
}