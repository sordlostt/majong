using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MapView : MonoBehaviour
{
    [SerializeField]
    Canvas canvas;
    [SerializeField]
    GameObject tileObject;

    [SerializeField]
    float tileWidth;
    [SerializeField]
    float tileHeight;

    public void DrawView(TileMap tileMap)
    {
        List <Tile> tiles = tileMap.GetTiles();

        foreach (Tile tile in tiles)
        {
            if (tile.GetTileType() != Tile.TileType.EMPTY)
            {
                Vector2Int tileMapDims = tileMap.GetDims();
                Vector2Int tileCoords = tile.GetCoords();
                GameObject tileObj = GameObject.Instantiate(tileObject, Vector3.zero, Quaternion.identity);
                RectTransform tileRect = tileObj.GetComponent<RectTransform>();
                Image tileImage = tileObj.GetComponentsInChildren<Image>()[1];

                float scale = GetTileScale(tileWidth * tileMap.GetDims().x, tileHeight * tileMap.GetDims().y);

                tileObj.transform.SetParent(canvas.transform);                
                tileRect.localScale = new Vector3(scale, scale, 1.0f);
                tileRect.localPosition = new Vector2(scale * (tileCoords.x - tileMapDims.x / 2.0f) * tileWidth, scale * (tileCoords.y - tileMapDims.y / 2.0f + 0.5f) * tileHeight);
            }
        }
    }

    private float GetTileScale(float w, float h)
    {
        return Mathf.Min(Mathf.Clamp01(Screen.width / (w * canvas.scaleFactor)), Mathf.Clamp01(Screen.height / (h * canvas.scaleFactor)));
    }
}
