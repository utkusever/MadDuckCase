using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConnectedBridgeView : MonoBehaviour
{
    [SerializeField] private Transform start;
    [SerializeField] private Transform end;
    [SerializeField] private MeshRenderer meshRenderer;

    [SerializeField] private float thickness;

    public void Bind(Transform a, Transform b)
    {
        start = a;
        end = b;
        UpdateVisual();
    }

    public void SetColors(Color colorA, Color colorB)
    {
        if (meshRenderer == null) return;
        var mats = meshRenderer.materials;
        mats[1].SetColor("_BaseColor", colorA);
        mats[0].SetColor("_BaseColor", colorB);
    }

    private void LateUpdate()
    {
        UpdateVisual();
    }

    private void UpdateVisual()
    {
        if (start == null || end == null)
        {
            gameObject.SetActive(false);
            return;
        }

        gameObject.SetActive(true);

        Vector3 startPosition = start.position;
        Vector3 endPosition = end.position;
        Vector3 dir = endPosition - startPosition;
        float len = Vector3.Distance(startPosition, endPosition);

        Debug.DrawLine(startPosition, endPosition, Color.green, 10f);

        transform.position = (startPosition + endPosition) * 0.5f;
        transform.rotation = Quaternion.LookRotation(dir.normalized, Vector3.up);

        transform.localScale = new Vector3(len / 2, thickness, thickness);
        transform.rotation *= Quaternion.Euler(0f, -90f, 0f); // gerekirse -90 dene
    }
}