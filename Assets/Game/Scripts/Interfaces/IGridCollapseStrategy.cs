using UnityEngine;

public interface IGridCollapseStrategy
{
    void Collapse(IMovableGridBall[,] occupancy, Vector3[,] worldPosProvider);
}