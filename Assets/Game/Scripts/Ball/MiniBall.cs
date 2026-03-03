using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Pool;
using Random = UnityEngine.Random;

public class MiniBall : MonoBehaviour
{
    [SerializeField] private Rigidbody myRigidBody;
    [SerializeField] private Collider myCollider;
    [SerializeField] private float seekSpeed;
    [SerializeField] private float homingStrength;
    [SerializeField] private MiniBallVisualController visualController;
    public IColorChanger ColorChanger => visualController;
    public Rigidbody Rigidbody => myRigidBody;
    private bool isSeeking;
    private Vector3 targetPos;
    private int bounceCount;
    private int targetBounceToSeek;
    private bool hasHit;
    private IObjectPool<MiniBall> miniBallPool;

    public ColorType ColorType { get; private set; }
    public ColorType GetColorType() => ColorType;
    public void SetColorType(ColorType colorType) => ColorType = colorType;

    private void Awake()
    {
        SetRandomTargetBounce();
    }


    private Vector2Int targetCell;
    private bool hasTarget;
    private IMiniBallTargetProvider targetProvider;

    private void OnDisable()
    {
        GameManager.Instance.DecreaseMiniBallCount();
    }

    public void SetPool(IObjectPool<MiniBall> objectPool)
    {
        miniBallPool = objectPool;
    }

    public void Reset()
    {
        targetCell = Vector2Int.zero;
        hasTarget = false;
        hasHit = false;
        isSeeking = false;
        targetPos = Vector3.zero;
        bounceCount = 0;
        SetColorType(ColorType.None);
        myCollider.enabled = true;
        myRigidBody.isKinematic = false;
        SetRandomTargetBounce();
        DOTween.Kill(transform);
        visualController.TrailRenderer.Clear();
    }

    public void SetTargetProvider(IMiniBallTargetProvider provider)
    {
        targetProvider = provider;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (hasHit) return;

        var collided = collision.collider;
        if (collided.TryGetComponent<ITargetableCube>(out var cube))
        {
            if (cube.IsDying) return; //unity engine :(((((((
            if (cube.ColorType == GetColorType())
            {
                hasHit = true;
                isSeeking = false;
                ReleaseTargetIfAny();
                myCollider.enabled = false;
                myRigidBody.velocity = Vector3.zero;
                myRigidBody.isKinematic = true;
                this.transform.DOScale(Vector3.zero, 0.1f).OnComplete(() => miniBallPool.Release(this));
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

    private void TryStartSeeking()
    {
        if (TryAcquireTarget())
            isSeeking = true;
    }

    private bool TryAcquireTarget()
    {
        if (targetProvider == null) return false;

        if (targetProvider.TryGetTarget(GetColorType(), transform.position, out var cell, out var pos))
        {
            targetCell = cell;
            targetPos = pos;
            hasTarget = true;
            return true;
        }

        hasTarget = false;
        return false;
    }


    private void FixedUpdate()
    {
        if (isSeeking)
        {
            // what if reserved target killed by someone else ?
            if (hasTarget && !targetProvider.IsTargetAlive(targetCell))
            {
                ReleaseTargetIfAny();
                if (!TryAcquireTarget())
                {
                    isSeeking = false;
                    return;
                }
            }

            Vector3 dir = (targetPos - transform.position).normalized;
            Vector3 desiredVelocity = dir * seekSpeed;
            Vector3 steering = desiredVelocity - myRigidBody.velocity;
            myRigidBody.AddForce(steering * homingStrength, ForceMode.Acceleration);
        }
        else
        {
            float current = myRigidBody.velocity.magnitude;
            if (current >= seekSpeed) return;
            Vector3 dir = myRigidBody.velocity.normalized;
            if (current < 0.1f) dir = transform.forward;
            float speedDeficit = seekSpeed - current;
            myRigidBody.AddForce(dir * speedDeficit, ForceMode.Acceleration);
        }
    }

    private void ReleaseTargetIfAny()
    {
        if (!hasTarget || targetProvider == null) return;
        targetProvider.ReleaseTarget(targetCell);
        hasTarget = false;
    }

    private void SetRandomTargetBounce()
    {
        targetBounceToSeek = Random.Range(1, 4);
    }
}