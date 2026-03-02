using System;
using UnityEngine;

public class BallGridOccupancy : BaseGridOccupancy<IMovableGridBall>
{
    public static BallGridOccupancy Instance;
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
        if (occupancy == null || worldPositions == null) return;
        collapseStrategy.Collapse(occupancy, worldPositions);
    }
}