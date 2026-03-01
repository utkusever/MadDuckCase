using UnityEngine;

public class BallAreaSetup : MonoBehaviour
{
    [SerializeField] private GameColorSO gameColorSo;
    [SerializeField] private BallSpawner ballSpawner;

    public void SetupLayout(ColorData[,] colorMap)
    {
        var generator = new BallLayoutGenerator(gameColorSo);
        var leveBallSlotDatas = generator.Generate(colorMap);
        ballSpawner.SpawnBalls(leveBallSlotDatas);
    }
}