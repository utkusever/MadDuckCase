public interface IColor
{
    public ColorType ColorType { get; }
    ColorType GetColorType();
    void SetColorType(ColorType colorType);
}