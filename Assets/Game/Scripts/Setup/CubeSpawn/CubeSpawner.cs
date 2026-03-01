using System;
using UnityEngine;

public abstract class CubeSpawner : MonoBehaviour
{
    [SerializeField] protected PixelCube cubePrefab;
    [SerializeField] protected Transform parent;
    [SerializeField] protected float spaceOffset = 1f;

    public abstract ICube[,] SpawnCubes(int rowCount, int columnCount);
}