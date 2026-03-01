using System;
using UnityEngine;

public class BallGridOccupancy : MonoBehaviour
{
    public static BallGridOccupancy Instance;
    private IBall[,] ballOccupancy;
    private Vector3[,] ballPositions;
    private IGridCollapseStrategy collapseStrategy;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }

        collapseStrategy = new GravityCollapseStrategy();
    }

    public void Collapse()
    {
        collapseStrategy.Collapse(ballOccupancy, ballPositions);
    }

    public void SetBallMatrix(int rowCount, int colCount)
    {
        ballOccupancy = new IBall[rowCount, colCount];
        ballPositions = new Vector3[rowCount, colCount];
    }

    public void SetWorldPosition(int row, int col, Vector3 pos)
    {
        ballPositions[row, col] = pos;
    }

    public void UnRegisterBall(IBall ball)
    {
        var ballCell = ball.Cell;
        ballOccupancy[ballCell.x, ballCell.y] = null;
    }

    public void RegisterBall(IBall ball)
    {
        var ballCell = ball.Cell;
        ballOccupancy[ballCell.x, ballCell.y] = ball;
    }

    public IBall GetAt(int row, int col)
    {
        if (ballOccupancy == null || row < 0 || row >= ballOccupancy.GetLength(0) || col < 0 ||
            col >= ballOccupancy.GetLength(1)) return null;
        return ballOccupancy[row, col];
    }

    public int RowCount => ballOccupancy?.GetLength(0) ?? 0;
    public int ColCount => ballOccupancy?.GetLength(1) ?? 0;
}