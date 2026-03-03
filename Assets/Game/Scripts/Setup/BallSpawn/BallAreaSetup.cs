using UnityEngine;

public class BallAreaSetup : MonoBehaviour
{
    [SerializeField] private GameColorSO gameColorSo;
    [SerializeField] private BallSpawner ballSpawner;
    [SerializeField] private bool assignRandomConnectedBalls;
    
    public void SetupLayout(ColorData[,] colorMap)
    {
        var generator = new BallLayoutGenerator(gameColorSo,assignRandomConnectedBalls);
        var leveBallSlotDatas = generator.Generate(colorMap);
        GameManager.Instance.SetTotalQueueBallCount(leveBallSlotDatas.Count);
        ballSpawner.SpawnBalls(leveBallSlotDatas);
    }
}