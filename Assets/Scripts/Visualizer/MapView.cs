using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MapView : Singleton<MapView>
{
    [SerializeField]
    Canvas canvas;

    [SerializeField]
    RectTransform tilesAnchor;

    [SerializeField]
    VisualizerConfig visualizerConfig;

    int columns;
    int rows;

    Dictionary<TileView, Tile> tiles = new Dictionary<TileView, Tile>();
    Dictionary<Tile, TileView> tileViews = new Dictionary<Tile, TileView>();

    public static event Action<Tile, Tile> OnPairPicked;
    public static event Action OnViewEmpty;

    TileView activeTileView = null;

    public void Init(TileMap tileMap)
    {
        TileMap.OnPairMatching += DestroyTilePair;
        TileMap.OnPairNotMatching += DeselectTile;

        List<Tile> tiles = tileMap.GetTiles();
        columns = tileMap.GetDims().x;
        rows = tileMap.GetDims().y;

        foreach (var tile in tiles)
        {
            if (tile.GetTileType() != Tile.TileType.EMPTY)
            {
                TileView tileView = Instantiate<GameObject>(visualizerConfig.TilePrefab, tilesAnchor).GetComponent<TileView>();

                foreach (var face in visualizerConfig.TileFaces)
                {
                    if (face.tileType == tile.GetTileType())
                    {
                        tileView.SetFace(face.sprite);
                    }
                }

                tileView.OnTileViewClicked += HandleTileViewClick;
                this.tiles.Add(tileView, tile);
                this.tileViews.Add(tile, tileView);
            }
        }
    }

    public void DrawView()
    {
        float scale = GetTileScale(visualizerConfig.TileWidth * columns, visualizerConfig.TileHeight * rows);

        foreach (var tileView in tileViews.Values)
        {
            tileView.transform.localScale = new Vector3(scale, scale, 1.0f);
            tileView.transform.localPosition = new Vector2(scale * visualizerConfig.TileWidth * (tiles[tileView].GetCoords().x - columns / 2.0f + 0.5f),
                                                           scale * visualizerConfig.TileHeight * (tiles[tileView].GetCoords().y - rows / 2.0f - 0.5f));
        }
    }

    private void HandleTileViewClick(TileView tileView)
    {
        if (activeTileView == null)
        {
            activeTileView = tileView;
            tileView.Highlight(true);
        }
        else if (tileView == activeTileView)
        {
            activeTileView = null;
            tileView.Highlight(false);
        }
        else
        {
            OnPairPicked?.Invoke(tiles[activeTileView],tiles[tileView]);
        }
    }

    private void DeselectTile()
    {
        activeTileView.Highlight(false);
        activeTileView = null;
    }

    private void DestroyTilePair(Tile tile1, Tile tile2)
    {
        tileViews[tile1].OnTileViewClicked -= HandleTileViewClick;
        tileViews[tile2].OnTileViewClicked -= HandleTileViewClick;
        Destroy(tileViews[tile1].gameObject);
        Destroy(tileViews[tile2].gameObject);
        tileViews.Remove(tile1);
        tileViews.Remove(tile2);
        activeTileView = null;
        if (tileViews.Count == 0)
        {
            OnViewEmpty?.Invoke();
        }
        DrawView();
    }

    private float GetTileScale(float w, float h)
    {
        return Mathf.Min(Mathf.Clamp01(Screen.width / (w * canvas.scaleFactor)), Mathf.Clamp01(Screen.height / (h * canvas.scaleFactor)));
    }
}
