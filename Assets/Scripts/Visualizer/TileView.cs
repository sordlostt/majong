using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TileView : MonoBehaviour
{
    Texture2D tileImage;
    Button tileButton;

    private void OnEnable()
    {
        tileButton = gameObject.GetComponentInChildren<Button>();
        tileButton.onClick.AddListener(OnTileClick);
    }

    private void OnTileClick()
    {
        Debug.Log("Click");
    }
}
