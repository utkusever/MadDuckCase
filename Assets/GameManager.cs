using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    private int activeMiniBallCount;
    private int remainingCubeCount;
    private int remainingQueueBallCount;
    private bool isGameOver;
    public bool IsGameOver => isGameOver;

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
        CheckFail();
    }

    public void DecreaseMiniBallCount()
    {
        activeMiniBallCount--;
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
        if (activeMiniBallCount > 50)
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
        print("FAIL");
        isGameOver = true;
    }

    private void Win()
    {
        print("WIN");
        isGameOver = true;
    }
}