using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class LayoutParser : Singleton<LayoutParser>
{
    TextAsset activeLayout;

    protected override void Awake()
    {
        base.Awake();
        DontDestroyOnLoad(gameObject);
        TitleUIHandler.OnLayoutPicked += SetActiveLayout;
    }

    int dimX;
    int dimY;

    public List<Vector2Int> GetLayout()
    {
        List<Vector2Int> result = new List<Vector2Int>();
        int lineCount = 0;
        using (var reader = new StringReader(activeLayout.text))
        {
            string line = reader.ReadLine();
            while (line != null && line != string.Empty)
            {
                for (int i = 0; i < line.Length; i++)
                {
                    if (line[i] == 'X')
                    {
                        result.Add(new Vector2Int(i, lineCount));
                    }
                    dimX = i;
                }
                line = reader.ReadLine();
                lineCount++;
            }
        }
        dimY = lineCount;
        return result;
    }

    public Vector2Int GetDims()
    {
        return new Vector2Int(dimX + 1, dimY);
    }

    private void SetActiveLayout(TextAsset layout)
    {
        activeLayout = layout;
    }
}
