using System;
using UnityEngine;

public class SimpleGridFromTexture : MonoBehaviour
{
    [SerializeField] private Texture2D texture;
    [SerializeField] private GameColorSO gameColorSo;

    private IColorMatch colorMatchStrategy;
    
    public ColorData[,] GenerateColorMap()
    {
        colorMatchStrategy ??= new DefaultColorMatchStrategy(gameColorSo);

        int width = texture.width;
        int height = texture.height;
        var colorMap = new ColorData[height, width];

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                Color pixelColor = texture.GetPixel(x, y);
                colorMap[y, x] = colorMatchStrategy.GetClosestColorData(pixelColor);
            }
        }

        return colorMap;
    }
}