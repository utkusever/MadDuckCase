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
                BallType = BallType.Normal
            });

        for (int i = 0; i < bigCount; i++)
            list.Add(new BallSlotData
            {
                ColorType = colorType,
                Color = color,
                BallType = BallType.Big
            });

        //AssignRandomConnected(list);

        return list;
    }

    private void GetBallCountsForColor(int pixelCount, out int normalCount, out int bigCount)
    {
        int unit = pixelCount / 10;
        int maxBig = unit / 2;

        bigCount = Random.Range(0, maxBig + 1);
        normalCount = unit - (bigCount * 2);
    }


    private void AssignRandomConnected(List<BallSlotData> list)
    {
        if (list.Count < 2) return;

        int groupCount = Random.Range(0, 3);

        var available = new List<int>();
        for (int i = 0; i < list.Count; i++)
            available.Add(i);

        for (int g = 0; g < groupCount; g++)
        {
            if (available.Count < 2) break;

            int first = TakeRandom(available);
            int second = TakeRandom(available);

            list[first].ConnectedGroupId = g + 1;
            list[second].ConnectedGroupId = g + 1;
        }
    }

    private int TakeRandom(List<int> list)
    {
        int idx = Random.Range(0, list.Count);
        int value = list[idx];
        list.RemoveAt(idx);
        return value;
    }
}