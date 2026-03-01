using System;
using DG.Tweening;
using UnityEngine;

public class Ball : MonoBehaviour, IBall
{
    [SerializeField] private VisualController visualController;

    public IColorChanger ColorChanger => visualController;
    [field: SerializeField] public Vector2Int Cell { get; set; }
    [field: SerializeField] public int Capacity { get; set; }
    [field: SerializeField] public int ConnectedGroupId { get; set; }

    public void MoveTo(Vector3 target)
    {
        this.transform.DOMove(target, 0.25f).SetEase(Ease.OutQuad);
    }

    public void JumpTo(Vector3 target, Action onComplete = null)
    {
        this.transform.DOJump(target, 10f, 1, 0.25f).OnComplete(() => onComplete?.Invoke());;
    }

    [field: SerializeField]  public ColorType ColorType { get; private set; }
    public ColorType GetColorType() => ColorType;
    public void SetColorType(ColorType colorType) => ColorType = colorType;
}