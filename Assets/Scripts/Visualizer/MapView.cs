using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MapView : MonoBehaviour
{
    [SerializeField]
    private Canvas canvas;

    [SerializeField]
    private RectTransform tilesAnchor;

    [SerializeField]
    private VisualizerConfig visualizerConfig;

    private int columns;
    private int rows;

    private Dictionary<TileView, Tile> tiles = new Dictionary<TileView, Tile>();
    private Dictionary<Tile, TileView> tileViews = new Dictionary<Tile, TileView>();

    private TileView activeTileView = null;

    public static event Action<Tile, Tile> OnPairPicked;
    public static event Action OnViewEmpty;


    public void Init(TileMap tileMap)
    {
        TileMap.OnPairMatching += DestroyTilePair;
        TileMap.OnPairNotMatching += DeselectTile;

        List<Tile> tiles = tileMap.Tiles;
        columns = tileMap.Dims.x;
        rows = tileMap.Dims.y;

        foreach (var tile in tiles)
        {
            if (tile.Type != Tile.TileType.EMPTY)
            {
                TileView tileView = Instantiate<GameObject>(visualizerConfig.TilePrefab, tilesAnchor).GetComponent<TileView>();

                foreach (var face in visualizerConfig.TileFaces)
                {
                    if (face.tileType == tile.Type)
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
            tileView.transform.localPosition = new Vector2(scale * visualizerConfig.TileWidth * (tiles[tileView].Coords.x - columns / 2.0f + 0.5f),
                                                           scale * visualizerConfig.TileHeight * (tiles[tileView].Coords.y - rows / 2.0f - 0.5f));
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
