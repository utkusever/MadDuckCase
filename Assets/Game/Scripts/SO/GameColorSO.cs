using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(menuName = "Create GameColorSO", fileName = "GameColorSO", order = 0)]
public class GameColorSO : ScriptableObject
{
    public List<ColorData> ColorDatas;

    public ColorData GetColorData(ColorType colorTypes)
    {
        return ColorDatas.First(x => x.ColorType == colorTypes);
    }
}

[Serializable]
public struct ColorData
{
    public ColorType ColorType;
    public Color CubeColor;

    public ColorData(ColorType colorType, Color cubeColor)
    {
        ColorType = colorType;
        CubeColor = cubeColor;
    }
}