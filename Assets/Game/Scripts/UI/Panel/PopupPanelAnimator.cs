using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class PopupPanelAnimator : IPanelAnimation
{
    private const float ScaleValue = 0.75f;
    private float fadeValue;
    private float animationDuration;
    private GameObject panel;
    private RectTransform rectTransform;
    private Image panelImage;
    private Vector3 stockScale;
    private Action OnOpen;
    private Action OnClose;

    public PopupPanelAnimator(GameObject panel, RectTransform rectTransform, Image panelImage, float fadeValue,
        float animationDuration, Action OnOpen, Action OnClose)
    {
        this.panel = panel;
        this.rectTransform = rectTransform;
        this.panelImage = panelImage;
        stockScale = rectTransform.localScale;
        this.fadeValue = fadeValue;
        this.animationDuration = animationDuration;
        this.OnOpen = OnOpen;
        this.OnClose = OnClose;
    }

    public void OpenPanel()
    {
        panel.SetActive(true);
        FadeImage(true);
        rectTransform.localScale *= ScaleValue;
        rectTransform.DOScale(stockScale, 0.2f).SetEase(Ease.OutCirc).OnComplete(() => OnOpen?.Invoke());
    }

    public void ClosePanel()
    {
        FadeImage(false);
        rectTransform.DOScale(stockScale * ScaleValue, animationDuration).SetEase(Ease.InCirc)
            .OnComplete(() => panel.SetActive(false));
    }

    private void FadeImage(bool open)
    {
        if (open)
        {
            panelImage.gameObject.SetActive(true);
            var panelImageColor = panelImage.color;
            panelImageColor.a = 0f;
            panelImage.color = panelImageColor;
            panelImage.DOFade(fadeValue, 0.4f);
        }
        else
        {
            panelImage.DOFade(0, animationDuration).OnComplete(() => panelImage.gameObject.SetActive(false));
        }
    }
}