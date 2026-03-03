using UnityEngine;

public class GameSetup : MonoBehaviour
{
    [SerializeField] private SimpleGridFromTexture simpleGridFromTexture;
    [SerializeField] private CubeSpawner cubeSpawner;
    [SerializeField] private BallAreaSetup ballAreaSetup;
    [SerializeField] private CubeGridOccupancy cubeGridOccupancy;

    private void Awake()
    {
        var colorMap = simpleGridFromTexture.GenerateColorMap();
        int width = colorMap.GetLength(1);
        int height = colorMap.GetLength(0);
        var spawnedCubes = cubeSpawner.SpawnCubes(width, height);
        GameManager.Instance.SetTotalCubeCount(spawnedCubes.Length);
        cubeGridOccupancy.Init(width, height);
        ballAreaSetup.SetupLayout(colorMap);
        FillCubeData(height, width, spawnedCubes, colorMap);
    }

    private void FillCubeData(int height, int width, ICube[,] spawnedCubes, ColorData[,] colorMap)
    {
        for (int row = 0; row < height; row++)
        {
            for (int col = 0; col < width; col++)
            {
                ICube cube = spawnedCubes[row, col];
                var data = colorMap[row, col];
                cube.SetColorType(data.ColorType);
                cube.ColorChanger.ChangeColor(data.CubeColor);
                FillCubeGridOccupancy(cube, row, col);
            }
        }
    }

    private void FillCubeGridOccupancy(ICube cube, int row, int col)
    {
        cube.Cell = new Vector2Int(row, col);
        cubeGridOccupancy.Register(cube, row, col);
        cubeGridOccupancy.SetWorldPosition(row, col, ((MonoBehaviour)cube).transform.position);
    }
}