using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class GameUIHandler : MonoBehaviour
{
    [Header("Gameplay UI")]
    [SerializeField]
    Button exitButton;
    [SerializeField]
    TMP_Text scoreText;

    [Header("Game Lost Panel")]
    [SerializeField]
    GameObject gameLostObject;
    [SerializeField]
    TMP_Text lostScoreText;
    [SerializeField]
    Button lostQuitButton;

    [Header("Game Won Panel")]
    [SerializeField]
    GameObject gameWonObject;
    [SerializeField]
    TMP_Text wonHighScoreText;
    [SerializeField]
    TMP_Text wonScoreText;
    [SerializeField]
    Button wonQuitButton;

    public event Action OnQuitButtonPressed;

    private void Awake()
    {
        gameLostObject.SetActive(false);
        gameWonObject.SetActive(false);
        exitButton.onClick.AddListener(DrawGameLost);
        GameManager.instance.OnGameWon += DrawGameWon;
        GameManager.instance.OnGameLost += DrawGameLost;
    }

    private void Update()
    {
        scoreText.text = $"Score: {GameManager.instance.Score}";
    }

    private void OnDisable()
    {
        GameManager.instance.OnGameWon -= DrawGameWon;
        GameManager.instance.OnGameLost -= DrawGameLost;
    }

    private void DrawGameLost()
    {
        scoreText.gameObject.SetActive(false);
        exitButton.gameObject.SetActive(false);
        gameLostObject.SetActive(true);
        lostScoreText.text = $"Score: {GameManager.instance.Score}";
        lostQuitButton.onClick.AddListener(() => OnQuitButtonPressed?.Invoke());
    }

    private void DrawGameWon()
    {
        scoreText.gameObject.SetActive(false);
        exitButton.gameObject.SetActive(false);
        gameWonObject.SetActive(true);
        wonScoreText.text = $"Score: {GameManager.instance.Score}";
        wonHighScoreText.text = $"Highscore: {PlayerData.instance.GetHighScore(GameManager.instance.LevelName)}";
        wonQuitButton.onClick.AddListener(() => OnQuitButtonPressed?.Invoke());
    }
}
