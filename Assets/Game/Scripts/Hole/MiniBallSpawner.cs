using System;
using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

public class MiniBallSpawner : MonoBehaviour
{
    [SerializeField] private BallToHoleController ballToHoleController;
    [SerializeField] private MiniBall miniBallPrefab;
    [SerializeField] private GameColorSO gameColorSo;
    private IMiniBallTargetProvider targetProvider;
    private WaitForSeconds waitForSeconds;
    private readonly float waitDelay = 0.01f;

    private void Awake()
    {
        ballToHoleController.OnBallDroppedIntoHole += Spawn;
        targetProvider = this.GetComponent<IMiniBallTargetProvider>();
        if (targetProvider == null)
        {
            Debug.LogError("No Target Provider Found In Mini Ball Spawner");
        }

        waitForSeconds = new WaitForSeconds(waitDelay);
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
        GameManager.Instance.IncreaseMiniBallCount(spawnCount);
    }

    private IEnumerator SpawnRoutine(int spawnCount, ColorType colorType)
    {
        var color = gameColorSo.GetColorData(colorType).CubeColor;

        for (int i = 0; i < spawnCount; i++)
        {
            var go = Instantiate(miniBallPrefab, transform.position, Quaternion.identity);
            go.SetColorType(colorType);
            go.ColorChanger.ChangeColor(color);
            go.SetTargetProvider(targetProvider);
            var rb = go.GetComponent<Rigidbody>();

            Vector3 baseDirection = transform.forward;

            float angle = Random.Range(-30f, 30f);

            Vector3 finalDirection = Quaternion.Euler(0, angle, 0) * baseDirection;

            float speed = Random.Range(20f, 25f);

            rb.velocity = finalDirection.normalized * speed;
            yield return waitForSeconds;
        }
    }
}