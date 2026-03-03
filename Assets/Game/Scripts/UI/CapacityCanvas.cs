using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using Sequence = DG.Tweening.Sequence;

public class CapacityCanvas : MonoBehaviour
{
    [SerializeField] private Image fillImage;
    [SerializeField] private TMP_Text capacityText;
    [SerializeField] private Transform capacityTextTransform;

    private int maxCapacity;
    private Color textBaseColor;
    private Vector3 textBaseScale;
    private int previousValue;

    private Tween fillTween;
    private Tween textColorTween;
    private Sequence textPunchSeq;

    private void Awake()
    {
        textBaseColor = capacityText.color;
        textBaseScale = capacityTextTransform.localScale;

        GameManager.Instance.OnActiveMiniBallCountChanged += FillBar;
        SetMaxCapacity(GameManager.Instance.MaxCapacity);
        SetText(0);
        FillBar(0);
    }

    private void OnDestroy()
    {
        GameManager.Instance.OnActiveMiniBallCountChanged -= FillBar;
        fillTween?.Kill();
        textColorTween?.Kill();
        textPunchSeq?.Kill();
    }

    private void SetMaxCapacity(int capacity)
    {
        maxCapacity = capacity;
    }

    private void FillBar(int value)
    {
        bool isIncreasing = value > previousValue;
        float ratio = (float)value / maxCapacity;
        SetText(value);
        fillTween?.Kill();
        fillTween = fillImage.DOFillAmount(ratio, 0.15f).SetEase(Ease.OutQuad);
        if (isIncreasing && ratio >= 0.8f)
        {
            textColorTween?.Kill();
            textColorTween = capacityText.DOColor(Color.red, 0.15f)
                .OnComplete(() => capacityText.DOColor(textBaseColor, 0.25f));
            textPunchSeq?.Kill();
            textPunchSeq = DOTween.Sequence();
            textPunchSeq.Append(capacityTextTransform.DOScale(textBaseScale * 1.1f, 0.2f).SetEase(Ease.OutQuad));
            textPunchSeq.Append(
                capacityTextTransform.DOScale(textBaseScale, 0.25f).SetEase(Ease.OutBack));
        }

        previousValue = value;
    }

    private void SetText(int value)
    {
        capacityText.text = value + " /" + maxCapacity;
    }
}