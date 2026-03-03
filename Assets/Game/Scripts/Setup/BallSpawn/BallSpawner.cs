using System.Collections.Generic;
using UnityEngine;

public class BallSpawner : MonoBehaviour
{
    [SerializeField] private BallObjectSO ballObjectSo;
    [SerializeField] protected Transform parent;
    [SerializeField] protected float spaceOffsetX;
    [SerializeField] private float spaceOffsetZ;
    [SerializeField] private int columnCount;

    [SerializeField] private BallGridOccupancy ballGridOccupancy;
    [SerializeField] private ConnectedBridgeManager connectedBridgeManager;

    public void SpawnBalls(List<BallSlotData> list)
    {
        ballGridOccupancy.Init(Mathf.CeilToInt((float)list.Count / columnCount), columnCount);
        var balls = new List<IBall>(list.Count);
        for (int i = 0; i < list.Count; i++)
        {
            int col = i % columnCount;
            int row = i / columnCount;
            var data = list[i];
            var ballData = ballObjectSo.GetBallData(data.BallType);
            var pos = GetPosition(col, row);
            var go = Instantiate(ballData.Prefab, pos, Quaternion.identity, parent);
            var ball = go.GetComponent<IBall>();
            ballGridOccupancy.SetWorldPosition(row, col, pos);
            if (ball != null)
            {
                ball.SetColorType(data.ColorType);
                ball.ColorChanger.ChangeColor(data.Color);
                ball.Cell = new Vector2Int(row, col);
                ball.Capacity = ballData.Capacity;
                ballGridOccupancy.Register(ball, row, col);
                balls.Add(ball);
            }
        }

        SetConnectedBallsData(list, balls);

        connectedBridgeManager.Build(balls);
    }

    private void SetConnectedBallsData(List<BallSlotData> list, List<IBall> balls)
    {
        var groups = new Dictionary<int, List<IBall>>();

        for (int i = 0; i < balls.Count; i++)
        {
            int id = list[i].ConnectedGroupId;
            if (id <= 0) continue; // no id assigned i mean its not a connected ball
            if (!groups.ContainsKey(id))
                groups[id] = new List<IBall>();
            groups[id].Add(balls[i]);
        }

        foreach (var kv in groups)
        {
            IBall a = kv.Value[0];
            IBall b = kv.Value[1];
            a.ConnectedBall = b;
            b.ConnectedBall = a;
        }
    }

    private Vector3 GetPosition(int col, int row)
    {
        return parent.transform.position + new Vector3(col * spaceOffsetX, 2f, -row * spaceOffsetZ);
    }
}


public class BallSlotData
{
    public ColorType ColorType;
    public Color Color;
    public BallType BallType;
    public int ConnectedGroupId;
}