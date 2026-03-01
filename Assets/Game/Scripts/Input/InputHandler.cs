using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputHandler : MonoBehaviour
{
    [SerializeField] private Camera cam;
    [SerializeField] private LayerMask ballLayerMask;

    public event Action<IBall> OnClickedBall;

    private void Awake()
    {
    }

    private void Update()
    {
        if (!Input.GetMouseButtonDown(0))
            return;

        if (cam == null)
            return;

        Ray ray = cam.ScreenPointToRay(Input.mousePosition);
        if (!Physics.Raycast(ray, out RaycastHit hit, float.MaxValue, ballLayerMask))
            return;

        var ball = hit.collider.GetComponent<IBall>();
        if (ball != null)
            OnClickedBall?.Invoke(ball);
    }
}