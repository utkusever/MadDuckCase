using UnityEngine;

public interface IColorMatch
{
    ColorData GetClosestColorData(Color pixelColor);
}