using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WinLosePanelController : MonoBehaviour
{
    [SerializeField] private PanelAnimate winPanel;
    [SerializeField] private PanelAnimate losePanel;


    private void Awake()
    {
        GameManager.Instance.OnWin += OpenWinPanel;
        GameManager.Instance.OnLose += OpenLosePanel;
    }

    private void OnDestroy()
    {
        GameManager.Instance.OnWin -= OpenWinPanel;
        GameManager.Instance.OnLose -= OpenLosePanel;
    }

    private void OpenWinPanel()
    {
        winPanel.RequestOpenPanel();
    }

    private void OpenLosePanel()
    {
        losePanel.RequestOpenPanel();
    }
    
}