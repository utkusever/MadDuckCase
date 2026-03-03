using UnityEngine;

public class MiniBallVisualController : VisualController
{
    [SerializeField] private TrailRenderer trailRenderer;
    public TrailRenderer TrailRenderer => trailRenderer;
    public override void ChangeColor(Color color)
    {
        base.ChangeColor(color);
        trailRenderer.startColor = color;
        trailRenderer.endColor = color;
    }
}