using System;
using DG.Tweening;
using UnityEngine;

public class CubeVisualController : VisualController
{
    [SerializeField] private Transform tweenTransform;

    private bool isHitTweenPlaying;
    private Material mat;
    private Color baseColor;
    private Color flashColor;
    private Vector3 baseScale;

    private void Awake()
    {
        baseScale = tweenTransform.localScale;
    }

    public override void ChangeColor(Color color)
    {
        base.ChangeColor(color);
        mat = myRenderer.material;
        baseColor = mat.GetColor("_BaseColor");
        flashColor = Color.Lerp(baseColor, Color.white, 0.4f);
    }

    public void PlayHitFeedback()
    {
        if (isHitTweenPlaying) return;
        isHitTweenPlaying = true;
        mat.DOColor(flashColor, "_BaseColor", 0.05f).OnComplete(() => mat.DOColor(baseColor, "_BaseColor", 0.09f));

        var s = DOTween.Sequence();
        s.Append(tweenTransform.DOScale(new Vector3(0.92f, 2f, 0.92f), 0.05f).SetEase(Ease.OutQuad));
        s.Append(tweenTransform.DOScale(new Vector3(1.08f, 0.7f, 1.08f), 0.06f).SetEase(Ease.InOutQuad));
        s.Append(tweenTransform.DOScale(baseScale, 0.07f).SetEase(Ease.OutBack));

        s.OnComplete(() => isHitTweenPlaying = false);
    }

    public void PlayDestroyEffect()
    {
        DOTween.Kill(this.tweenTransform);
        mat.DOColor(flashColor, "_BaseColor", 0.05f).OnComplete(() => mat.DOColor(baseColor, "_BaseColor", 0.09f));

        var s = DOTween.Sequence();
        s.Append(tweenTransform.DOScale(new Vector3(0.92f, 2f, 0.92f), 0.05f).SetEase(Ease.OutQuad));
        s.Append(tweenTransform.DOScale(new Vector3(1.08f, 0.7f, 1.08f), 0.06f).SetEase(Ease.InOutQuad));
        s.Append(tweenTransform.DOScale(Vector3.zero, 0.07f).SetEase(Ease.OutBack))
            .OnComplete(() => Destroy(gameObject));
    }
}