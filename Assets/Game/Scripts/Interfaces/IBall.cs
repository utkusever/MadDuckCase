using UnityEngine;

public interface IBall : IMove, IColor
{
    IColorChanger ColorChanger { get; }
    Vector2Int Cell { get; set; }
    int Capacity { get; set; }
    int ConnectedGroupId { get; set; }
}