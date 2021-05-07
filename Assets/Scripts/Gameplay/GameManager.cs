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
    private GameUIHandler UIHandler;

    [SerializeField]
    private float scoreReward;

    [SerializeField]
    private float scorePenalty;

    private float score;

    private string levelName;

    private TileMap tileMap = new TileMap();

    public float Score { get => score; }

    public string LevelName { get => levelName; }

    public event Action OnGameWon;
    public event Action OnGameLost;

    public event Action<Tile, Tile> OnPairMatched;
    public event Action<Tile> OnHintFound;
    public event Action OnPairNotMatched;

    private Action<Tile, Tile> notifyViewOnPairMatched;
    private Action<Tile> notifyViewOnHintFound;
    private Action notifyViewOnPairNotMatched;

    protected override void Awake()
    {
        base.Awake();

        notifyViewOnPairMatched = (Tile tile, Tile tile1) => { OnPairMatched?.Invoke(tile, tile1); };
        notifyViewOnHintFound = (Tile tile) => { OnHintFound?.Invoke(tile); };
        notifyViewOnPairNotMatched = () => { OnPairNotMatched?.Invoke(); };
        RandomValues.Init();
        tileMap.InitializeMap();
        mapView.Init(tileMap);

        levelName = LayoutParser.instance.ActiveLayout.name;

        UIHandler.OnQuitButtonPressed += EndLevel;
        mapView.OnViewEmpty += GameWon;
        tileMap.OnPairMatched += AddScore;
        tileMap.OnPairNotMatched += DeductScore;
        tileMap.OnPairNotMatched += notifyViewOnPairNotMatched;
        tileMap.OnPairMatching += notifyViewOnPairMatched;
        tileMap.OnHintFound += notifyViewOnHintFound;
        tileMap.OnHintNotFound += GameLost;
    }

    private void OnDisable()
    {
        tileMap.OnPairMatched -= AddScore;
        tileMap.OnPairNotMatched -= DeductScore;
        tileMap.OnPairNotMatched -= notifyViewOnPairNotMatched;
        tileMap.OnPairMatching -= notifyViewOnPairMatched;
        tileMap.OnHintFound -= notifyViewOnHintFound;
        tileMap.OnHintNotFound -= GameLost;
        UIHandler.OnQuitButtonPressed -= EndLevel;
        mapView.OnViewEmpty -= GameWon;
    }

    private void Start()
    {
        mapView.DrawView();
    }

    private void EndLevel()
    {
        SceneTransitionManager.instance.LoadTitle();
    }

    private void GameWon()
    {
        PlayerData.instance.RefreshHighScore(score, levelName);
        PlayerData.instance.SetLevelCompleted(levelName);
        OnGameWon?.Invoke();
    }

    private void GameLost()
    {
        OnGameLost?.Invoke();
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
}
