using System;
using UnityEngine;
using UnityEngine.UI;

public class PanelAnimate : MonoBehaviour, IPanel
{
    [SerializeField] private GameObject mainPanelGameObject;
    [SerializeField] private RectTransform panelRectTransform;
    [SerializeField] private Image panelBackgroundImage;
    [SerializeField] private float fadeValue = 0.55f;
    [SerializeField] private float animationDuration = 0.2f;

    private IPanelAnimation panelAnimation;

    public Action OnPanelOpened { get; set; }
    public Action OnPanelClosed { get; set; }

    public void RequestOpenPanel()
    {
        panelAnimation ??=
            new PopupPanelAnimator(mainPanelGameObject, panelRectTransform, panelBackgroundImage, fadeValue,
                animationDuration, OnPanelOpened, OnPanelClosed);
        panelAnimation.OpenPanel();
    }

    public void RequestClosePanel()
    {
        panelAnimation.ClosePanel();
    }
}