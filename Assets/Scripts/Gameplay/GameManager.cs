using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : Singleton<GameManager>
{
    float score;
    TileMap tileMap;
    [SerializeField]
    MapView mapView;

    public static event Action OnGameWon;
    public static event Action OnGameLost;

    protected override void Awake()
    {
        base.Awake();
        RandomValues.Init();

        GameUIHandler.OnGameQuit += EndLevel;

        tileMap = new TileMap();
        tileMap.InitializeMap();
        mapView.DrawView(tileMap);
    }

    private void EndLevel()
    {
        GameUIHandler.OnGameQuit -= EndLevel;
        SceneManager.LoadScene("Title");
    }

    public float GetScore()
    {
        return score;
    }
}
