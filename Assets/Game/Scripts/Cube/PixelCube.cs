using System;
using UnityEngine;

public class PixelCube : MonoBehaviour, ICube, ITargetableCube
{
    [SerializeField] private CubeVisualController visualController;
    [SerializeField] private Collider myCollider;

    
    //Interface methods
    public event Action<IGridCube> OnDestroyed;
    [field: SerializeField] public ColorType ColorType { get; private set; }

    public void Hit()
    {
        visualController.PlayHitFeedback();
    }
    
    public ColorType GetColorType() => ColorType;
    public void SetColorType(ColorType colorType) => ColorType = colorType;
    public IColorChanger ColorChanger => visualController;
    public Vector2Int Cell { get; set; }

    public void DestroyObject()
    {
        myCollider.enabled = false;
        OnDestroyed?.Invoke(this);
        visualController.PlayDestroyEffect();
    }
}