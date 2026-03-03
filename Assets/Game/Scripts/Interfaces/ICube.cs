using System;

public interface ICube : IGridCube, IColor
{
    IColorChanger ColorChanger { get; }
}

public interface IDestroyable
{
    void DestroyObject();
}

public interface IDestroyEvent
{
    event Action<IGridCube> OnDestroyed;
}

public interface IGridCube : IGridCoordinate, IDestroyEvent
{
    ColorType ColorType { get; }
}

public interface ITargetableCube : IDestroyable
{
    bool IsDying { get; }
    ColorType ColorType { get; }
    void Hit();
}