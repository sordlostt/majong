using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class TileView : MonoBehaviour, IPointerClickHandler
{
    [SerializeField]
    private Image tileBack;
    [SerializeField]
    private Image tileFace;
    [SerializeField]
    private VisualizerConfig visualizerConfig;

    public event Action<TileView> OnTileViewClicked;

    public void SetFace(Sprite tex)
    {
        tileFace.sprite = tex;
    }

    public void Highlight(bool enabled)
    {
        tileBack.color = enabled ? visualizerConfig.TileHighlightColor : Color.white;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        OnTileViewClicked?.Invoke(this);
    }
}
