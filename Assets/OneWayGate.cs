using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OneWayGate : MonoBehaviour
{
    [SerializeField] private Transform inwardRef; // arena yönünü gösteren transform
    [SerializeField] private float minInwardSpeed = 6f;

    private void OnTriggerStay(Collider other)
    {
        var mini = other.GetComponent<MiniBall>();
        if (mini == null) return;

        var rb = mini.GetComponent<Rigidbody>();
        if (rb == null) return;

        Vector3 inward = inwardRef.forward.normalized;
        float dot = Vector3.Dot(rb.velocity, inward);

        if (dot < 0f) // dışarı gidiyor
        {
            Vector3 tangent = Vector3.ProjectOnPlane(rb.velocity, inward);
            rb.velocity = tangent + inward * minInwardSpeed;
        }
    }
}