using System.Collections.Generic;
using UnityEngine;

public class BallLayoutGenerator
{
    private GameColorSO gameColorSo;

    public BallLayoutGenerator(GameColorSO gameColorSo)
    {
        this.gameColorSo = gameColorSo;
    }
    
    public List<BallSlotData> Generate(ColorData[,] colorMap)
    {
        var colorCounts = CountColors(colorMap);
        var allBalls = new List<BallSlotData>();

        foreach (var kvp in colorCounts)
        {
            var list = CreateBallsForColor(kvp.Key, kvp.Value);
            allBalls.AddRange(list);
        }

        return allBalls;
    }

    private Dictionary<ColorType, int> CountColors(ColorData[,] colorMap)
    {
        int h = colorMap.GetLength(0), w = colorMap.GetLength(1);
        var counts = new Dictionary<ColorType, int>();
        for (int row = 0; row < h; row++)
        {
            for (int col = 0; col < w; col++)
            {
                var t = colorMap[row, col].ColorType;
                if (t == ColorType.None) continue;
                if (counts.ContainsKey(t))
                {
                    counts[t]++;
                }
                else
                {
                    counts.Add(t, 1);
                }
            }
        }


        return counts;
    }

    private const float NormalRatio = 0.7f; // %70 Normal (10), %30 Big (20)
    private const int CapacityNormal = 10;
    private const int CapacityBig = 20;


    private void GetBallCountsForColor(int pixelCount, out int normalCount, out int bigCount)
    {
        int needCapacity = pixelCount; // en az bu kadar
        float avgCapacity = NormalRatio * CapacityNormal + (1f - NormalRatio) * CapacityBig; // ~13
        int totalBalls = Mathf.Max(1, Mathf.CeilToInt(needCapacity / avgCapacity));

        normalCount = Mathf.RoundToInt(totalBalls * NormalRatio);
        bigCount = totalBalls - normalCount;
        if (bigCount < 0)
        {
            bigCount = 0;
            normalCount = totalBalls;
        }

        int totalCap = normalCount * CapacityNormal + bigCount * CapacityBig;
        while (totalCap < needCapacity)
        {
            if (Random.value < NormalRatio)
            {
                normalCount++;
                totalCap += CapacityNormal;
            }
            else
            {
                bigCount++;
                totalCap += CapacityBig;
            }
        }
    }

    private List<BallSlotData> CreateBallsForColor(ColorType colorType, int pixelCount)
    {
        GetBallCountsForColor(pixelCount, out int normalCount, out int bigCount);

        Color color = gameColorSo.GetColorData(colorType).CubeColor;

        var list = new List<BallSlotData>(normalCount + bigCount);
        for (int i = 0; i < normalCount; i++)
            list.Add(new BallSlotData
            {
                ColorType = colorType,
                Color = color,
                BallType = BallType.Normal,
                ConnectedGroupId = 0
            });
        for (int i = 0; i < bigCount; i++)
            list.Add(new BallSlotData
            {
                ColorType = colorType,
                Color = color,
                BallType = BallType.Big,
                ConnectedGroupId = 0
            });

        AssignRandomConnected(list, minGroups: 3, maxGroups: 5);
        return list;
    }

    private void AssignRandomConnected(List<BallSlotData> list, int minGroups, int maxGroups)
    {
        if (list.Count < 2) return;
        int groupCount = Random.Range(minGroups, maxGroups + 1);
        groupCount = Mathf.Min(groupCount, list.Count / 2); // en az 2'li grup

        var indices = new List<int>(list.Count);
        for (int i = 0; i < list.Count; i++) indices.Add(i);

        for (int g = 0; g < groupCount; g++)
        {
            int groupSize = Random.Range(2, Mathf.Max(2, indices.Count / (groupCount - g) + 1));
            groupSize = Mathf.Min(groupSize, indices.Count);
            if (groupSize < 2 || indices.Count < 2) break;

            for (int k = 0; k < groupSize && indices.Count > 0; k++)
            {
                int idx = Random.Range(0, indices.Count);
                int listIndex = indices[idx];
                indices.RemoveAt(idx);
                var b = list[listIndex];
                b.ConnectedGroupId = g + 1;
                list[listIndex] = b;
            }
        }
    }
}