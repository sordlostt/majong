using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TileMap
{
    private int columns;
    private int rows;

    private Dictionary<Vector2Int, Tile> tiles = new Dictionary<Vector2Int, Tile>();

    public List<Tile> Tiles { get => new List<Tile>(tiles.Values); }
    public Vector2Int Dims { get => new Vector2Int(columns, rows); }

    public event Action<Tile, Tile> OnPairMatching;
    public event Action<Tile> OnHintFound;
    public event Action OnPairMatched;
    public event Action OnPairNotMatched;
    public event Action OnHintNotFound;

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
                tile.Coords = new Vector2Int(j, i);
                tile.Type = Tile.TileType.EMPTY;
                tiles.Add(tile.Coords, tile);
            }
        }

        // randomize tiles until there is a move available at the start
        do
        {
            GenerateRandomTiles(layout);
        } while (!LookForHints(true));
    }

    private bool LookForHints(bool levelStart)
    {
        bool hintFound = false;
        Tile hint = null;

        foreach (var tile in Tiles)
        {
            if (tile.Type != Tile.TileType.EMPTY)
            {
                hint = Tiles.FirstOrDefault(x => x.Type == tile.Type && x != tile);
                if (FindPath(tile, hint))
                {
                    hint = tile;
                    hintFound = true;
                    break;
                }
            }
        }

        if (!levelStart)
        {
            if (hintFound)
            {
                OnHintFound?.Invoke(hint);
            }
            else
            {
                OnHintNotFound?.Invoke();
            }
        }

        return hintFound;
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

    private void CheckPair(Tile origin, Tile target)
    {
        if (FindPath(origin, target))
        {
            RemoveLayoutTile(origin);
            RemoveLayoutTile(target);
            OnPairMatching?.Invoke(origin, target);
            OnPairMatched?.Invoke();
        }
        else
        {
            OnPairNotMatched?.Invoke();
            LookForHints(false);
        }
    }

    private bool FindPath(Tile origin, Tile target)
    {
        if (origin.Type != target.Type)
        {
            return false;
        }

        Vector2Int start = origin.Coords;
        Vector2Int end = target.Coords;

        if (start.x == end.x)
        {
            return  CheckLine(start.x, Mathf.Min(start.y, end.y), Mathf.Max(start.y, end.y), horizontal:false)
                 || CheckShapes(start, end);
        }
        else if (start.y == end.y)
        {
            return  CheckLine(start.y, Mathf.Min(start.x, end.x), Mathf.Max(start.x, end.x), horizontal:true)
                 || CheckShapes(start, end);
        }
        else
        {
            return CheckShapes(start, end);
        }
    }

    /* check if there are any S, L or U shaped paths that would take us to your target
     * since we can only make 2 turns, only S,L,U shapes are a viable movement option*/
    private bool CheckShapes(Vector2Int start, Vector2Int end)
    {
        Vector2Int leftMost = start.x < end.x ? start : end;
        Vector2Int rightMost = start.x > end.x ? start : end;
        Vector2Int upMost = start.y < end.y ? start : end;
        Vector2Int downMost = start.y > end.y ? start : end;

        // S shapes
        int leftIdx = -1;
        int rightIdx = -1;

        if (CheckRange(leftMost, 1, true, ref leftIdx) & CheckRange(rightMost, -1, true, ref rightIdx))
        {
            for (int i = rightIdx; i <= leftIdx; i++)
            {
                if (CheckLine(i, Mathf.Min(start.y, end.y), Mathf.Max(start.y, end.y), false))
                {
                    return true;
                }
            }
        }

        int upIdx = -1;
        int downIdx = -1;

        if (CheckRange(upMost, 1, false, ref upIdx) & CheckRange(downMost, -1, false, ref downIdx))
        {
            for (int i = downIdx; i <= upIdx; i++)
            {
                if (CheckLine(i, Mathf.Min(start.x, end.x), Mathf.Max(start.x, end.x), true))
                {
                    return true;
                }
            }
        }

        if (leftIdx > -1 && leftIdx >= rightIdx)
        {
            // L shape

            if (CheckLine(rightMost.x, upMost.y, downMost.y, false))
            {
                return true;
            }

            // U shape

            int rightIdx2 = -1;
            if (CheckRange(rightMost, 1, true, ref rightIdx2))
            {
                for (int i = Mathf.Min(leftIdx, rightMost.x + 1); i <= Mathf.Min(leftIdx, rightIdx2); i++)
                {
                    if (CheckLine(i, Mathf.Min(start.y, end.y), Mathf.Max(start.y, end.y), false))
                    {
                        return true;
                    }
                }
            }
        }
        else if (rightIdx > -1 && rightIdx <= leftMost.x)
        {
            // L shape

            if (CheckLine(leftMost.x, upMost.y, downMost.y, false))
            {
                return true;
            }

            // U shape

            int leftIdx2 = -1;
            if (CheckRange(leftMost, -1, true, ref leftIdx2))
            {
                for (int i = Mathf.Min(rightIdx, leftMost.x - 1); i >= Mathf.Min(rightIdx, leftIdx2); i--)
                {
                    if (CheckLine(i, Mathf.Min(start.y, end.y), Mathf.Max(start.y, end.y), false))
                    {
                        return true;
                    }
                }
            }
        }

        if (upIdx > -1 && upIdx > downMost.y)
        {
            // U shape
            int downIdx2 = -1;
            if (CheckRange(downMost, 1, false, ref downIdx2))
            {
                for (int i = Mathf.Min(upIdx, downMost.y + 1); i <= Mathf.Min(upIdx, downIdx2); i++)
                {
                    if (CheckLine(i, Mathf.Min(start.x, end.x), Mathf.Min(start.x, end.x), true))
                    {
                        return true;
                    }
                }
            }
        }
        else if (downIdx > -1 && downIdx < upMost.y)
        {
            // U shape
            int upIdx2 = -1;
            if (CheckRange(upMost, -1, false, ref upIdx2))
            {
                for (int i = Mathf.Min(downIdx, downMost.y - 1); i >= Mathf.Max(downIdx, upIdx2); i--)
                {
                    if (CheckLine(i, Mathf.Min(start.x, end.x), Mathf.Min(start.x, end.x), true))
                    {
                        return true;
                    }
                }
            }
        }

        return false;
    }

    // check if we can reach the destination by a straight line walk
    private bool CheckLine(int line, int min, int max, bool horizontal)
    {
        Vector2Int coords = Vector2Int.zero ;

        for (int i = min + 1; i < max; i++)
        {
            coords.x = horizontal ? i : line;
            coords.y = horizontal ? line : i;

            if (tiles[coords].Type != Tile.TileType.EMPTY)
            {
                return false;
            }
        }

        return true;
    }

    // check how far we can go
    private bool CheckRange(Vector2Int start, int step, bool horizontal, ref int idx)
    {
        int end;
        int walk = (horizontal ? start.x : start.y) + step;
        Vector2Int walkCoords = new Vector2Int();

        if (step > 0)
        {
            end = horizontal ? columns : rows;
        }
        else
        {
            end = 0;
        }

        while (true)
        {
            if ((step > 0 && walk >= end) || (step < 0 && walk < end))
            {
                break;
            }

            walkCoords.x = horizontal ? walk : start.x;
            walkCoords.y = horizontal ? start.y : walk;

            if (tiles[walkCoords].Type != Tile.TileType.EMPTY)
            {
                return idx > -1;
            }
            else
            {
                idx = walk;
            }

            walk += step;
        }

        return idx > -1;
    }

    private void RemoveLayoutTile(Tile tile)
    {
        tiles[tile.Coords].Type = Tile.TileType.EMPTY;
    }

}
