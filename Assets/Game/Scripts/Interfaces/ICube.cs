public interface ICube : IColor
{
    IColorChanger ColorChanger { get; }
    void DestroyCube();
}