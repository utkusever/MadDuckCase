using UnityEngine;

public class DefaultColorMatchStrategy : IColorMatch
{
    private readonly GameColorSO gameColorSo;

    public DefaultColorMatchStrategy(GameColorSO gameColorSo)
    {
        this.gameColorSo = gameColorSo;
    }

    public ColorData GetClosestColorData(Color pixelColor)
    {
        float minDistance = float.MaxValue;
        ColorData closest = default;

        foreach (var entry in gameColorSo.ColorDatas)
        {
            float distance = Vector3.Distance(
                new Vector3(pixelColor.r, pixelColor.g, pixelColor.b),
                new Vector3(entry.CubeColor.r, entry.CubeColor.g, entry.CubeColor.b));

            if (distance < minDistance)
            {
                minDistance = distance;
                closest = entry;
            }
        }

        return closest;
    }
}