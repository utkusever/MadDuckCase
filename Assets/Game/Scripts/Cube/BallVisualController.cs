using TMPro;
using UnityEngine;

public class BallVisualController : VisualController
{
    [SerializeField] private TMP_Text text;

    public void UpdateCapacityText(int capacity)
    {
        text.text = capacity.ToString();
    }
}