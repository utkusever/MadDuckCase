using UnityEngine;

public class VisualController : MonoBehaviour, IColorChanger
{
    [SerializeField] private Renderer myRenderer;

    public void ChangeColor(Color color)
    {
        var mat = myRenderer.material;
        mat.color = color;
        myRenderer.material = mat;
    }
}
