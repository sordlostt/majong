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

    private TileMap tileMap = new TileMap();

    public float Score { get => score; }

    public event Action OnGameWon;
    public event Action OnGameQuit;

    public event Action<Tile, Tile> OnPairMatched;
    public event Action OnPairNotMatched;

    private Action<Tile, Tile> notifyViewOnPairMatched;
    private Action notifyViewOnPairNotMatched;

    protected override void Awake()
    {
        base.Awake();

        notifyViewOnPairMatched = (Tile tile, Tile tile1) => { OnPairMatched?.Invoke(tile, tile1); };
        notifyViewOnPairNotMatched = () => { OnPairNotMatched?.Invoke(); };

        RandomValues.Init();
        tileMap.InitializeMap();
        mapView.Init(tileMap);

        UIHandler.OnQuitButtonPressed += EndLevel;
        mapView.OnViewEmpty += GameWon;
        tileMap.OnPairMatched += AddScore;
        tileMap.OnPairNotMatched += DeductScore;
        tileMap.OnPairNotMatched += notifyViewOnPairNotMatched;
        tileMap.OnPairMatching += notifyViewOnPairMatched;
    }

    private void OnDisable()
    {
        if (!Application.isLoadingLevel)
        {
            tileMap.OnPairMatched -= AddScore;
            tileMap.OnPairNotMatched -= DeductScore;
            tileMap.OnPairNotMatched -= notifyViewOnPairNotMatched;
            tileMap.OnPairMatching -= notifyViewOnPairMatched;
            UIHandler.OnQuitButtonPressed -= EndLevel;
            mapView.OnViewEmpty -= GameWon;
        }
    }

    private void Start()
    {
        mapView.DrawView();
    }

    private void EndLevel()
    {
        OnGameQuit?.Invoke();
        SceneTransitionManager.instance.LoadTitle();
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
}
