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
        base.Register(item, row, col);
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

    public bool IsReserved(int row, int col) => IsInside(row, col) && reserved[row, col];

    public void SetReserved(int row, int col, bool value)
    {
        if (!IsInside(row, col)) return;
        reserved[row, col] = value;
    }

    public bool TryGetBottomTarget(ColorType color, Vector3 fromPos, out Vector2Int cell, out Vector3 pos)
    {
        cell = default;
        pos = default;

        float bestDist = float.MaxValue;
        bool found = false;

        for (int col = 0; col < ColCount; col++)
        {
            // alttan üste tara: o sütundaki en alt uygun küp
            for (int row = RowCount - 1; row >= 0; row--)
            {
                var cube = occupancy[row, col];
                if (cube == null) continue;
                if (cube.ColorType != color) continue;
                if (reserved[row, col]) continue;

                Vector3 wp = worldPositions[row, col];
                float d = (wp - fromPos).sqrMagnitude;
                if (d < bestDist)
                {
                    bestDist = d;
                    cell = new Vector2Int(row, col);
                    pos = wp;
                    found = true;
                }

                break; // bu sütunda en altı bulduk, üste çıkma
            }
        }

        if (found) reserved[cell.x, cell.y] = true;
        return found;
    }
}