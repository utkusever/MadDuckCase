using System;
using DG.Tweening;
using UnityEngine;

public class Ball : MonoBehaviour, IBall
{
    [SerializeField] private BallVisualController visualController;
    [SerializeField] private int capacity;

    public IColorChanger ColorChanger => visualController;
    public Vector2Int Cell { get; set; }

    public int Capacity
    {
        get => capacity;
        set
        {
            capacity = value;
            visualController.UpdateCapacityText(capacity);
        }
    }

    public Transform Transform => this.transform;

    public void MoveTo(Vector3 target)
    {
        this.transform.DOMove(target, 0.25f).SetEase(Ease.OutQuad);
    }

    public void JumpTo(Vector3 target, Action onComplete = null)
    {
        this.transform.DOJump(target, 20f, 1, 0.25f).OnComplete(() =>
        {
            onComplete?.Invoke();
            Destroy(this.gameObject);
        });
    }

    public ColorType ColorType { get; private set; }
    public ColorType GetColorType() => ColorType;
    public void SetColorType(ColorType colorType) => ColorType = colorType;
    [field: SerializeField] public IBall ConnectedBall { get; set; }
    public bool IsConnected => ConnectedBall != null;

    public void FlyToHole(Vector3 holePoint, Action onComplete = null)
    {
        Vector3 start = transform.position;
        Vector3 sink = holePoint;

        Vector3 midPoint = (start + holePoint) * 0.3f;
        midPoint.y = start.y + 15f;

        Vector3[] path = { midPoint, sink };

        transform
            .DOPath(path, 0.4f, PathType.CatmullRom)
            .SetEase(Ease.InOutQuad)
            .OnComplete(() =>
            {
                onComplete?.Invoke();
                Destroy(gameObject);
            });
    }
}