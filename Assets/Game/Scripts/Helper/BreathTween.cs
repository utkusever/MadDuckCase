using System;
using DG.Tweening;
using UnityEngine;

public class BreathTween : MonoBehaviour
{
    private Tween breathTween;
    private Vector3 originalLocalScale;
    [SerializeField] private float duration;
    [SerializeField] private Transform transformToTween;

    private void Start()
    {
        StartBreathTween();
    }

    private void StartBreathTween()
    {
        originalLocalScale = transformToTween.localScale;
        breathTween = transformToTween.DOScale(originalLocalScale * 1.05f, duration)
            .SetLoops(-1, LoopType.Yoyo)
            .SetEase(Ease.InOutSine);
    }

    public void StopBreathTween()
    {
        if (breathTween == null) return;
        breathTween.Kill();
        breathTween = null;
    }
}