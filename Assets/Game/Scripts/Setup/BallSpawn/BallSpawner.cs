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

    public void SpawnBalls(List<BallSlotData> list)
    {
        ballGridOccupancy.Init(Mathf.CeilToInt((float)list.Count / columnCount), columnCount);

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
                ball.ConnectedGroupId = data.ConnectedGroupId;
                ballGridOccupancy.Register(ball, row, col);
            }
        }
    }

    private Vector3 GetPosition(int col, int row)
    {
        return parent.transform.position + new Vector3(col * spaceOffsetX, 2f, -row * spaceOffsetZ);
    }
}


public struct BallSlotData
{
    public ColorType ColorType;
    public Color Color;
    public int ConnectedGroupId;
    public BallType BallType;
}