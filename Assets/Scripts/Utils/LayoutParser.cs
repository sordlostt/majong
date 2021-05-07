using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class LayoutParser : Singleton<LayoutParser>
{
    [SerializeField]
    char tileChar = 'X';

    public TextAsset ActiveLayout { get; set; }

    List<Vector2Int> tileCoords;
    Vector2Int gridDimensions;


    protected override void Awake()
    {
        base.Awake();
        DontDestroyOnLoad(this);
    }

    public void ParseLayout()
    {
        tileCoords = new List<Vector2Int>();

        using (var reader = new StringReader(ActiveLayout.text))
        {
            string line = reader.ReadLine();

            if (line != null && line != string.Empty)
            {
                gridDimensions.x = line.Length;
                gridDimensions.y = 0;
            }

            while (line != null && line != string.Empty)
            {
                for (int i = 0; i < line.Length; i++)
                {
                    if (line[i] == tileChar)
                    {
                        tileCoords.Add(new Vector2Int(i + 1, gridDimensions.y + 1));
                    }
                }

                gridDimensions.y++;
                line = reader.ReadLine();
            }
        }
    }

    public List<Vector2Int> GetLayout()
    {
        List<Vector2Int> result = new List<Vector2Int>();

        // convert coordinates to proper format
        foreach (var coordinate in tileCoords)
        {
            result.Add(new Vector2Int(coordinate.x, gridDimensions.y + 1 - coordinate.y));
        }

        return result;
    }

    public Vector2Int GetDims()
    {
        // add 2 to the borders so that we can travel around the board
        return new Vector2Int(gridDimensions.x + 2, gridDimensions.y + 2);
    }
}
