using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TileMap
{
    int columns;
    int rows;

    Dictionary<Vector2Int, Tile> tiles = new Dictionary<Vector2Int, Tile>();

    public TileMap()
    {
        MapView.OnPairPicked += CheckPair;
    }

    public void InitializeMap()
    {
        var layout = LayoutParser.instance.GetLayout();
        var dims = LayoutParser.instance.GetDims();
        columns = dims.x;
        rows = dims.y;

        for (int i = 0; i < dims.y; i++)
        {
            for (int j = 0; j < dims.x; j++)
            {
                Tile tile = new Tile();
                tile.SetCoords(new Vector2Int(j, i));
                tile.SetEmptyTileType();
                tiles.Add(tile.GetCoords(), tile);
            }
        }

        GenerateRandomTiles(layout);
    }

    private void GenerateRandomTiles(IEnumerable<Vector2Int> layout)
    {
        var coords = layout.ToList();

        if (coords.Count % 2 != 0)
        {
            Debug.LogError("Layout has an uneven amount of tiles!");
        }

        List<Vector2Int> coords1 = RandomValues.ShuffleCollection(coords.GetRange(0, coords.Count / 2)).ToList();
        List<Vector2Int> coords2 = RandomValues.ShuffleCollection(coords.GetRange(coords.Count / 2, coords.Count / 2)).ToList();

        int tileTypeCount = Enum.GetValues(typeof(Tile.TileType)).Length - 1;

        for (int i = 0; i < coords1.Count; i++)
        {
            tiles[coords1[i]].Type = (Tile.TileType)(i % tileTypeCount);
            tiles[coords2[i]].Type = (Tile.TileType)(i % tileTypeCount);
        }
    }

    private void CheckPair(Tile origin, Tile destination)
    {

    }

    private void DestroyTile(Tile tile)
    {
        tiles[tile.GetCoords()].SetEmptyTileType();
    }

    public List<Tile> GetTiles()
    {
        return new List<Tile>(tiles.Values);
    }

    public Vector2Int GetDims()
    {
        return new Vector2Int(columns, rows);
    }



}
