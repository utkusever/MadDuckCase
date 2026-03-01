using UnityEngine;

public class GameSetup : MonoBehaviour
{
    [SerializeField] private SimpleGridFromTexture simpleGridFromTexture;
    [SerializeField] private CubeSpawner cubeSpawner;
    [SerializeField] private BallAreaSetup ballAreaSetup;

    private void Awake()
    {
        var colorMap = simpleGridFromTexture.GenerateColorMap();
        int width = colorMap.GetLength(1);
        int height = colorMap.GetLength(0);
        var cubes = cubeSpawner.SpawnCubes(width, height);
        ballAreaSetup.SetupLayout(colorMap);
        for (int row = 0; row < height; row++)
        {
            for (int col = 0; col < width; col++)
            {
                ICube cube = cubes[row, col];
                var data = colorMap[row, col];
                cube.SetColorType(data.ColorType);
                cube.ColorChanger.ChangeColor(data.CubeColor);
            }
        }
    }
}