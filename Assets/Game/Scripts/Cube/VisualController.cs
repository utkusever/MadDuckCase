using UnityEngine;

public class VisualController : MonoBehaviour, IColorChanger
{
    [SerializeField] protected Renderer myRenderer;

    public virtual void ChangeColor(Color color)
    {
        myRenderer.material.SetColor("_BaseColor", color);
    }
}