using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile
{
    public enum TileType
    {
        Bamboo1,
        Bamboo2,
        Bamboo3,
        Bamboo4,
        Bamboo5,
        Bamboo6,
        Bamboo7,
        Bamboo8,
        Bamboo9,
        Characters1,
        Characters2,
        Characters3,
        Characters4,
        Characters5,
        Characters6,
        Characters7,
        Characters8,
        Characters9,
        Dots1,
        Dots2,
        Dots3,
        Dots4,
        Dots5,
        Dots6,
        Dots7,
        Dots8,
        Dots9,
        Dragons1,
        Dragons2,
        Dragons3,
        Flowers1,
        Flowers2,
        Flowers3,
        Flowers4,
        Seasons1,
        Seasons2,
        Seasons3,
        Seasons4,
        Winds1,
        Winds2,
        Winds3,
        Winds4,
        EMPTY
    }

    public TileType Type { get; set; }

    public Vector2Int Coords { get; set; }
}
