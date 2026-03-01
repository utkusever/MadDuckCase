using UnityEngine;

public class DefaultCubeSpawner : CubeSpawner
{
    public override ICube[,] SpawnCubes(int rowCount, int columnCount)
    {
        var grid = new ICube[rowCount, columnCount];

        for (int row = 0; row < rowCount; row++)
        {
            for (int col = 0; col < columnCount; col++)
            {
                var pos = parent.transform.position + new Vector3(col * spaceOffset, 0f, row * spaceOffset);
                var go = Instantiate(cubePrefab, pos, Quaternion.identity, parent);
                go.name = $"Cube_{row}_{col}";
                grid[row, col] = go.GetComponent<ICube>();
            }
        }

        return grid;
    }
}