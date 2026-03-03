using UnityEngine;

public class CubeGridOccupancy : BaseGridOccupancy<IGridCube>
{
    private bool[,] reserved;

    public override void Init(int rowCount, int colCount)
    {
        base.Init(rowCount, colCount);
        reserved = new bool[rowCount, colCount];
    }

    public override void Register(IGridCube item, int row, int col)
    {
        base.Register(item, row, col);
        item.OnDestroyed += HandleCubeDestroyed;
    }

    public override void Unregister(int row, int col)
    {
        if (!IsInside(row, col)) return;
        var cube = occupancy[row, col];
        cube.OnDestroyed -= HandleCubeDestroyed;
        base.Unregister(row, col);
        reserved[row, col] = false;
    }

    private void HandleCubeDestroyed(IGridCube cube)
    {
        Unregister(cube.Cell.x, cube.Cell.y);
        print("Destroyed: " + cube.Cell.x + " , " + cube.Cell.y);
    }

    public void ReleaseReservation(Vector2Int cell)
    {
        reserved[cell.x, cell.y] = false;
    }

    public bool IsCellAlive(Vector2Int cell)
    {
        return occupancy[cell.x, cell.y] != null;
    }

    public bool TryGetBottomTarget(ColorType color, Vector3 ballPos, out Vector2Int cell, out Vector3 pos)
    {
        cell = default;
        pos = default;

        float bestDist = float.MaxValue;
        bool found = false;
        //look for each col's bottom row
        for (int col = 0; col < ColCount; col++)
        {
            for (int row = 0; row < RowCount; row++)
            {
                var cube = occupancy[row, col];
                if (cube == null) continue; // if cube is null go look for upper cube

                // if there is a cube check it
                if (cube.ColorType == color && !reserved[row, col])
                {
                    Vector3 worldPosition = worldPositions[row, col];
                    float distance = (worldPosition - ballPos).sqrMagnitude;
                    if (distance < bestDist)
                    {
                        bestDist = distance;
                        cell = new Vector2Int(row, col);
                        pos = worldPosition;
                        found = true;
                    }
                }

                // yeah I founded the bottom cube in this col break it
                break;
            }
        }

        if (found)
        {
            SetReserved(cell.x, cell.y);
        }

        return found;
    }

    private void SetReserved(int row, int col)
    {
        reserved[row, col] = true;
    }
}