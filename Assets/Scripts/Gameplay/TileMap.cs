using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileMap
{
    int columns;
    int rows;

    Dictionary<Vector2Int, Tile> tiles = new Dictionary<Vector2Int, Tile>();

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

        foreach (Vector2Int coordinate in layout)
        {
            tiles[coordinate].SetRandomTileType();
        }
    }

    public List<Tile> GetTiles()
    {
        List<Tile> result = new List<Tile>();
        foreach (var tile in tiles.Values)
        {
            result.Add(tile);
        }
        return result;
    }

    public Vector2Int GetDims()
    {
        return new Vector2Int(columns, rows);
    }



}
