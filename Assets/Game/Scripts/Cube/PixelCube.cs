using UnityEngine;

public class PixelCube : MonoBehaviour, ICube
{
    [SerializeField] private CubeVisual cubeVisual;
    [field: SerializeField] public ColorType ColorType { get; private set; }
    public ColorType GetColorType() => ColorType;
    public void SetColorType(ColorType colorType) => ColorType = colorType;
    public IColorChanger ColorChanger => cubeVisual;
}