using UnityEngine;

public interface IBall : IColor, IMovableGridBall, IConnectableBall
{
    IColorChanger ColorChanger { get; }
    int Capacity { get; set; }
}

public interface IMovableGridBall : IGridCoordinate, IMove
{
}

public interface IConnectableBall
{
    IBall ConnectedBall { get; set; }
    bool IsConnected { get; }
    Transform Transform { get; }
    ColorType ColorType { get; }
}