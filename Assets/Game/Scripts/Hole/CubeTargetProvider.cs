using UnityEngine;

public class CubeTargetProvider : MonoBehaviour, IMiniBallTargetProvider
{
    [SerializeField] private CubeGridOccupancy cubeGridOccupancy;

    public bool TryGetTarget(ColorType colorType, Vector3 fromPos, out Vector2Int cell, out Vector3 targetPos)
    {
        cell = default;
        targetPos = default;
        if (cubeGridOccupancy == null) return false;
        return cubeGridOccupancy.TryGetBottomTarget(colorType, fromPos, out cell, out targetPos);
    }

    public bool IsTargetAlive(Vector2Int cell)
    {
        return cubeGridOccupancy.IsCellAlive(cell);
    }

    public void ReleaseTarget(Vector2Int cell)
    {
        cubeGridOccupancy.ReleaseReservation(cell);
    }
}

public interface IMiniBallTargetProvider
{
    bool TryGetTarget(ColorType colorType, Vector3 fromPos, out Vector2Int cell, out Vector3 targetPos);
    bool IsTargetAlive(Vector2Int cell);
    void ReleaseTarget(Vector2Int cell);
}