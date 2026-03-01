using UnityEngine;

public class DefaultCubeSpawner : CubeSpawner
{
    private string namePrefix = "Cube";

    public override ICube[,] SpawnCubes(int rowCount, int columnCount)
    {
        var grid = new ICube[rowCount, columnCount];

        for (int row = 0; row < rowCount; row++)
        {
            for (int col = 0; col < columnCount; col++)
            {
                var pos = Vector3(col, row);
                var go = Instantiate(cubePrefab, pos, Quaternion.identity, parent);
                go.name = name + row + "," + col;
                grid[row, col] = go;
            }
        }

        return grid;
    }

    private Vector3 Vector3(int col, int row)
    {
        var pos = parent.transform.position + new Vector3(col * spaceOffset, 0f, row * spaceOffset);
        return pos;
    }
}