using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : Singleton<GameManager>
{
    [SerializeField]
    private MapView mapView;

    [SerializeField]
    private float scoreReward;

    [SerializeField]
    private float scorePenalty;

    private float score;

    private TileMap tileMap = new TileMap();

    public static event Action OnGameWon;
    public static event Action OnGameLost;

    protected override void Awake()
    {
        base.Awake();

        RandomValues.Init();
        tileMap.InitializeMap();
        mapView.Init(tileMap);

        GameUIHandler.OnGameQuit += EndLevel;
        TileMap.OnPairMatched += AddScore;
        TileMap.OnPairNotMatching += DeductScore;
        MapView.OnViewEmpty += GameWon;
    }

    private void Start()
    {
        mapView.DrawView();
    }

    private void EndLevel()
    {
        GameUIHandler.OnGameQuit -= EndLevel;
        SceneManager.LoadScene("Title");
    }

    private void GameWon()
    {
        PlayerData.instance.RefreshHighScore(score);
        PlayerData.instance.SetLevelCompleted(LayoutParser.instance.ActiveLayout.name);
        OnGameWon?.Invoke();
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
