public interface IBall : IColor, IMovableGridBall
{
    IColorChanger ColorChanger { get; }
    int Capacity { get; set; }
    int ConnectedGroupId { get; set; }
}

public interface IMovableGridBall : IGridCoordinate, IMove
{
}