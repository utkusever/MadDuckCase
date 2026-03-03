using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ConnectedBridgeManager : MonoBehaviour
{
    [SerializeField] private ConnectedBridgeView connectedBridgePrefab;
    [SerializeField] private Transform bridgeParent;
    [SerializeField] private GameColorSO gameColorSo;

    private readonly Dictionary<IBall, ConnectedBridgeView> activeBridges = new();

    public void Build(List<IBall> balls)
    {
        ClearAll();


        for (int i = 0; i < balls.Count; i++)
        {
            IBall currentBall = balls[i];
            if (currentBall == null) continue;
            if (!currentBall.IsConnected) continue;

            IBall other = currentBall.ConnectedBall;
            if (other == null) continue;

            if (activeBridges.ContainsKey(currentBall)) continue;
            if (activeBridges.ContainsKey(other)) continue;

            CreateBridgeForPair(currentBall, other);
        }
    }

    public void ClearAll()
    {
        foreach (var kv in activeBridges)
        {
            if (kv.Value != null)
                Destroy(kv.Value.gameObject);
        }

        activeBridges.Clear();
    }


    public void RemoveGroup(IBall ball)
    {
        if (ball == null) return;
        if (!activeBridges.TryGetValue(ball, out var bridge)) return;

        IBall other = ball.ConnectedBall;

        if (bridge != null)
            Destroy(bridge.gameObject);

        activeBridges.Remove(ball);
        if (other != null)
            activeBridges.Remove(other);
    }

    private void CreateBridgeForPair(IBall a, IBall b)
    {
        var bridge = Instantiate(connectedBridgePrefab, bridgeParent);
        bridge.Bind(a.Transform, b.Transform);
        Color colorA = gameColorSo.GetColorData(a.GetColorType()).CubeColor;
        Color colorB = gameColorSo.GetColorData(b.GetColorType()).CubeColor;
        bridge.SetColors(colorA, colorB);
        activeBridges[a] = bridge;
        activeBridges[b] = bridge;
    }
}