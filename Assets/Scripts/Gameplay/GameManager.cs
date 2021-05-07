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

    [SerializeField]
    float scoreReward;

    [SerializeField]
    float scorePenalty;

    public static event Action OnGameWon;
    public static event Action OnGameLost;

    protected override void Awake()
    {
        base.Awake();
        RandomValues.Init();

        GameUIHandler.OnGameQuit += EndLevel;
        TileMap.OnPairMatched += AddScore;
        TileMap.OnPairNotMatching += DeductScore;
        MapView.OnViewEmpty += () => { OnGameWon?.Invoke(); };

        tileMap = new TileMap();
        tileMap.InitializeMap();
        mapView.Init(tileMap);
        mapView.DrawView();
    }

    private void EndLevel()
    {
        GameUIHandler.OnGameQuit -= EndLevel;
        SceneManager.LoadScene("Title");
    }

    private void AddScore()
    {
        score += scoreReward;
    }

    private void DeductScore()
    {
        score += scorePenalty;
        if (score < 0.0f)
            score = 0.0f;
    }

    public float GetScore()
    {
        return score;
    }
}
