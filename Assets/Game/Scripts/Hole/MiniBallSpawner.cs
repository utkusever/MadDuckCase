using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Pool;
using Random = UnityEngine.Random;

public class MiniBallSpawner : MonoBehaviour
{
    [SerializeField] private BallToHoleController ballToHoleController;
    [SerializeField] private MiniBall miniBallPrefab;
    [SerializeField] private GameColorSO gameColorSo;
    private IMiniBallTargetProvider targetProvider;
    private WaitForSeconds waitForSeconds;
    private readonly float waitDelay = 0.01f;
    private IObjectPool<MiniBall> miniBallPool;
    public IObjectPool<MiniBall> MiniBallPool => miniBallPool;

    private void Awake()
    {
        ballToHoleController.OnBallDroppedIntoHole += Spawn;
        targetProvider = this.GetComponent<IMiniBallTargetProvider>();
        if (targetProvider == null)
        {
            Debug.LogError("No Target Provider Found In Mini Ball Spawner Object");
        }

        waitForSeconds = new WaitForSeconds(waitDelay);
        SetUpMiniBallPool();
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
            //var go = Instantiate(miniBallPrefab, transform.position, Quaternion.identity);
            var go = miniBallPool.Get();

            go.SetColorType(colorType);
            go.ColorChanger.ChangeColor(color);

            Vector3 baseDirection = transform.forward;

            float angle = Random.Range(-30f, 30f);

            Vector3 finalDirection = Quaternion.Euler(0, angle, 0) * baseDirection;

            float speed = Random.Range(20f, 25f);

            go.Rigidbody.velocity = finalDirection.normalized * speed;
            yield return waitForSeconds;
        }
    }

    private void SetUpMiniBallPool()
    {
        miniBallPool = new ObjectPool<MiniBall>(
            CreatePool,
            OnGetFromPool,
            OnReturnToPool,
            OnDestroyPooledObject,
            maxSize: 100
        );
    }

    private MiniBall CreatePool()
    {
        var miniBall = Instantiate(miniBallPrefab);
        miniBall.SetPool(miniBallPool);
        miniBall.SetTargetProvider(targetProvider);
        return miniBall;
    }

    private void OnGetFromPool(MiniBall miniBall)
    {
        miniBall.Reset();
        miniBall.transform.position = transform.position;
        miniBall.transform.rotation = Quaternion.identity;
        miniBall.transform.localScale = Vector3.one;
        miniBall.gameObject.SetActive(true);
    }

    private void OnReturnToPool(MiniBall miniBall)
    {
        miniBall.gameObject.SetActive(false);
    }

    private void OnDestroyPooledObject(MiniBall miniBall)
    {
        Destroy(miniBall.gameObject);
    }
}

public class MiniBallPool : MonoBehaviour
{
}