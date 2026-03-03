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
            go.Rigidbody.position = transform.position;
            go.Rigidbody.rotation = Quaternion.identity;
            go.transform.localScale = Vector3.one;
            go.SetColorType(colorType);
            go.ColorChanger.ChangeColor(color);
            go.SetTargetProvider(targetProvider);
            Vector3 baseDirection = transform.forward;

            float angle = Random.Range(-30f, 30f);

            Vector3 finalDirection = Quaternion.Euler(0, angle, 0) * baseDirection;

            float speed = Random.Range(20f, 25f);
            go.Rigidbody.velocity = finalDirection.normalized * speed;
            
            yield return new WaitForFixedUpdate();
            go.ClearTrail();
            go.SetTrailEmitting(true);
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
        
        miniBall.SetTrailEmitting(false);
        miniBall.ClearTrail();
        
        miniBall.Rigidbody.position = transform.position;
        miniBall.Rigidbody.rotation = Quaternion.identity;
        miniBall.Rigidbody.velocity = Vector3.zero;
        miniBall.Rigidbody.angularVelocity = Vector3.zero;
        miniBall.transform.localScale = Vector3.one;
        miniBall.TrailRenderer.Clear();
        miniBall.gameObject.SetActive(true);
    }

    private void OnReturnToPool(MiniBall miniBall)
    {
        miniBall.SetTrailEmitting(false);
        miniBall.ClearTrail();
        miniBall.gameObject.SetActive(false);
        miniBall.ClearTrail(); 
    }

    private void OnDestroyPooledObject(MiniBall miniBall)
    {
        Destroy(miniBall.gameObject);
    }
}

public class MiniBallPool : MonoBehaviour
{
}