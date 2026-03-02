using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using Random = UnityEngine.Random;

public class MiniBall : MonoBehaviour, IMiniBall
{
    [SerializeField] private Rigidbody myRigidBody;
    [SerializeField] private Collider myCollider;
    [SerializeField] private float seekSpeed = 12f;
    [SerializeField] private float homingStrength = 4f;
    [SerializeField] private VisualController visualController;
    public IColorChanger ColorChanger => visualController;

    private bool isSeeking;
    private Vector3 targetPos;
    private int bounceCount;
    private int targetBounceToSeek;
    private bool hasHit;

    public ColorType ColorType { get; private set; }
    public ColorType GetColorType() => ColorType;
    public void SetColorType(ColorType colorType) => ColorType = colorType;

    private void Awake()
    {
        targetBounceToSeek = Random.Range(2, 5);
    }

    public void SetGrid()
    {
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (hasHit) return;

        var collided = collision.collider;
        if (collided.TryGetComponent<ITargetableCube>(out var cube))
        {
            if (cube.ColorType == GetColorType())
            {
                hasHit = true;
                isSeeking = false;
                myCollider.enabled = false;
                myRigidBody.velocity = Vector3.zero;
                myRigidBody.isKinematic = true;
                this.transform.DOScale(Vector3.zero, 0.1f).OnComplete(() => Destroy(this.gameObject));
                cube.DestroyObject();
                return;
            }

            cube.Hit();
        }


        if (collided.CompareTag("Wall"))
        {
            bounceCount++;

            if (!isSeeking && bounceCount >= targetBounceToSeek)
            {
                TryStartSeeking();
            }
        }
    }

    private void FixedUpdate()
    {
        if (!isSeeking) return;

        Vector3 dir = (targetPos - transform.position).normalized;
        myRigidBody.velocity = Vector3.Lerp(
            myRigidBody.velocity,
            dir * seekSpeed,
            homingStrength * Time.fixedDeltaTime
        );
    }

    private void TryStartSeeking()
    {
        // var bottomCell = GridManager.Instance.GetBottomCell(myColor);
        //
        // if (bottomCell == null)
        //     return;
        //
        // targetPos = bottomCell.WorldPosition;
        // isSeeking = true;
    }
}

public interface IMiniBall : IColor
{
    IColorChanger ColorChanger { get; }
}