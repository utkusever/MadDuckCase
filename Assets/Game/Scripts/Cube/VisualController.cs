using UnityEngine;

public class VisualController : MonoBehaviour, IColorChanger
{
    [SerializeField] private Renderer myRenderer;

    public void ChangeColor(Color color)
    {
        myRenderer.material.SetColor("_BaseColor", color);
    }
}