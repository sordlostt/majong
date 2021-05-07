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

    private TileView currentTileView = null;
    private TileView currentHintTileView = null;

    public static event Action<Tile, Tile> OnPairPicked;
    public event Action OnViewEmpty;

    public void Init(TileMap tileMap)
    {
        GameManager.instance.OnPairMatched += DestroyTilePair;
        GameManager.instance.OnPairNotMatched += DeselectTile;
        GameManager.instance.OnHintFound += HighlightHint;

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

                tileView.OnTileViewClicked += TileViewClickHandler;
                this.tiles.Add(tileView, tile);
                tileViews.Add(tile, tileView);
            }
        }
    }

    private void OnDisable()
    {
        GameManager.instance.OnPairMatched -= DestroyTilePair;
        GameManager.instance.OnPairNotMatched -= DeselectTile;
        GameManager.instance.OnHintFound -= HighlightHint;

        foreach (var tileView in tileViews.Values)
        {
            tileView.OnTileViewClicked -= TileViewClickHandler;
        }
    }

    public void DrawView()
    {
        float scale = GetTileScale(visualizerConfig.TileWidth * columns, visualizerConfig.TileHeight * rows);

        foreach (var tileView in tileViews.Values)
        {
            tileView.transform.localScale = new Vector3(scale, scale, 1.0f);
            tileView.transform.localPosition = new Vector2(scale * visualizerConfig.TileWidth * (tiles[tileView].Coords.x - columns / 2.0f + visualizerConfig.TilePositioningOffsetX),
                                                           scale * visualizerConfig.TileHeight * (tiles[tileView].Coords.y - rows / 2.0f) + visualizerConfig.TilePositioningOffsetY);
        }
    }

    private void TileViewClickHandler(TileView tileView)
    {
        if (currentTileView == null)
        {
            currentTileView = tileView;
            tileView.HighlightAsCurrent(true);
        }
        else if (tileView == currentTileView)
        {
            currentTileView = null;
            tileView.HighlightAsCurrent(false);
        }
        else
        {
            OnPairPicked?.Invoke(tiles[currentTileView],tiles[tileView]);
        }
    }

    private void HighlightHint(Tile hint)
    {
        if (currentHintTileView != null)
        {
            currentHintTileView.HighlightAsHint(false);
        }

        currentHintTileView = tileViews[hint];
        currentHintTileView.HighlightAsHint(true);
    }

    private void DeselectTile()
    {
        currentTileView.HighlightAsCurrent(false);
        currentTileView = null;
    }

    private void DestroyTilePair(Tile tile1, Tile tile2)
    {
        tileViews[tile1].OnTileViewClicked -= TileViewClickHandler;
        tileViews[tile2].OnTileViewClicked -= TileViewClickHandler;
        Destroy(tileViews[tile1].gameObject);
        Destroy(tileViews[tile2].gameObject);
        tileViews.Remove(tile1);
        tileViews.Remove(tile2);
        currentTileView = null;
        if (tileViews.Count == 0)
        {
            OnViewEmpty?.Invoke();
        }
        DrawView();
    }

    private float GetTileScale(float w, float h)
    {
        float widthClamped = Mathf.Clamp01(Screen.width / w);
        float heightClamped = Mathf.Clamp01(Screen.height / h);
        return Mathf.Min(widthClamped, heightClamped);
    }
}
