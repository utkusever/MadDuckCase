using UnityEngine;

public class GravityCollapseStrategy : IGridCollapseStrategy
{
    public void Collapse(IMovableGridBall[,] occupancy, Vector3[,] worldPosProvider)
    {
        int rowCount = occupancy.GetLength(0);
        int colCount = occupancy.GetLength(1);

        for (int col = 0; col < colCount; col++)
        {
            int currentFillRow = 0; // Doldurulacak ilk satır (ön)

            for (int row = 0; row < rowCount; row++)
            {
                IMovableGridBall ball = occupancy[row, col];
                if (ball != null)
                {
                    if (row != currentFillRow)
                    {
                        occupancy[currentFillRow, col] = ball;
                        occupancy[row, col] = null;
                        ball.Cell = new Vector2Int(currentFillRow, col);
                        ball.MoveTo(worldPosProvider[currentFillRow, col]);
                    }

                    currentFillRow++;
                }
            }
        }
    }
}