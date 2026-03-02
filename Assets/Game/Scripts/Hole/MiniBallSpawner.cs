using System;
using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

public class MiniBallSpawner : MonoBehaviour
{
    [SerializeField] private BallToHoleController ballToHoleController;
    [SerializeField] private MiniBall miniBallPrefab;
    [SerializeField] private GameColorSO gameColorSo;

    private void Awake()
    {
        ballToHoleController.OnBallDroppedIntoHole += Spawn;
    }

    private void OnDestroy()
    {
        ballToHoleController.OnBallDroppedIntoHole -= Spawn;
    }

    private void Spawn(IBall ball)
    {
        var spawnCount = ball.Capacity;
        var colorType = ball.GetColorType();
        StartCoroutine(SpawnRoutine(spawnCount, colorType));
    }

    private IEnumerator SpawnRoutine(int spawnCount, ColorType colorType)
    {
        var color = gameColorSo.GetColorData(colorType).CubeColor;

        for (int i = 0; i < spawnCount; i++)
        {
            var go = Instantiate(miniBallPrefab, transform.position, Quaternion.identity);

            go.SetColorType(colorType);
            go.ColorChanger.ChangeColor(color);

            var rb = go.GetComponent<Rigidbody>();

            // Yukarı yön (arena yönüne göre değiştir)
            Vector3 baseDirection = transform.forward;

            // Cone açısı
            float angle = Random.Range(-30f, 30f);

            // Y ekseni etrafında rastgele sapma
            Vector3 finalDirection = Quaternion.Euler(0, angle, 0) * baseDirection;

            float speed = Random.Range(20f, 25f);

            rb.velocity = finalDirection.normalized * speed;
            yield return new WaitForSeconds(0.01f);
        }
    }
}