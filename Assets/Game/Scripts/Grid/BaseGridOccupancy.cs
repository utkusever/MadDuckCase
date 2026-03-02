using UnityEngine;

public abstract class BaseGridOccupancy<T> : MonoBehaviour where T : class
{
    protected T[,] occupancy;
    protected Vector3[,] worldPositions;

    public int RowCount => occupancy?.GetLength(0) ?? 0;
    public int ColCount => occupancy?.GetLength(1) ?? 0;

    public virtual void Init(int rowCount, int colCount)
    {
        occupancy = new T[rowCount, colCount];
        worldPositions = new Vector3[rowCount, colCount];
    }

    public bool IsInside(int row, int col)
    {
        return occupancy != null &&
               row >= 0 && row < occupancy.GetLength(0) &&
               col >= 0 && col < occupancy.GetLength(1);
    }

    public virtual void SetWorldPosition(int row, int col, Vector3 pos)
    {
        if (!IsInside(row, col)) return;
        worldPositions[row, col] = pos;
    }

    public virtual Vector3 GetWorldPosition(int row, int col)
    {
        if (!IsInside(row, col)) return default;
        return worldPositions[row, col];
    }

    public virtual T GetAt(int row, int col)
    {
        if (!IsInside(row, col)) return null;
        return occupancy[row, col];
    }

    public virtual void Register(T item, int row, int col)
    {
        if (!IsInside(row, col)) return;
        occupancy[row, col] = item;
    }

    public virtual void Unregister(int row, int col)
    {
        if (!IsInside(row, col)) return;
        occupancy[row, col] = null;
    }

    public virtual void ClearAll()
    {
        if (occupancy == null) return;

        for (int r = 0; r < occupancy.GetLength(0); r++)
        for (int c = 0; c < occupancy.GetLength(1); c++)
            occupancy[r, c] = null;
    }

    public T[,] GetGridUnsafe() => occupancy;
    public Vector3[,] GetWorldPositionsUnsafe() => worldPositions;
}