using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "VisualizerConfig", menuName = "Configs/VisualizerConfig", order = 1)]
public class VisualizerConfig : ScriptableObject
{
    [System.Serializable]
    public struct TileFace
    {
        public Sprite sprite;
        public Tile.TileType tileType;
    }

    [SerializeField]
    private List<TileFace> tileFaces = new List<TileFace>();

    [SerializeField]
    private float tileWidth;

    [SerializeField]
    private float tileHeight;

    [SerializeField]
    private float tilePositioningOffsetX;

    [SerializeField]
    private float tilePositioningOffsetY;

    [SerializeField]
    private GameObject tilePrefab;

    [SerializeField]
    private Color tileHighlightColor;

    [SerializeField]
    private Color tileHintColor;

    public List<TileFace> TileFaces { get => tileFaces; }

    public float TileWidth { get => tileWidth; }

    public float TileHeight { get => tileHeight; }

    public float TilePositioningOffsetX { get => tilePositioningOffsetX; }

    public float TilePositioningOffsetY { get => tilePositioningOffsetY; }

    public GameObject TilePrefab { get => tilePrefab; }

    public Color TileHighlightColor { get => tileHighlightColor; }

    public Color TileHintColor { get => tileHintColor; }

}
