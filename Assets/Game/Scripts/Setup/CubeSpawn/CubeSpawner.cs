using System;
using UnityEngine;

public abstract class CubeSpawner : MonoBehaviour
{
    [SerializeField] protected GameObject cubePrefab;
    [SerializeField] protected Transform parent;
    [SerializeField] protected float spaceOffset = 1f;

    public abstract ICube[,] SpawnCubes(int rowCount, int columnCount);
}