using System;

public interface IPanel
{
    void RequestOpenPanel();
    void RequestClosePanel();
    Action OnPanelOpened { get; set; }
    Action OnPanelClosed { get; set; }
}