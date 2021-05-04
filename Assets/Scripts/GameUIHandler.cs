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
    TMP_Text wonScoreText;
    [SerializeField]
    Button wonQuitButton;

    public static event Action OnGameQuit;

    private void Awake()
    {
        gameLostObject.SetActive(false);
        gameWonObject.SetActive(false);
        exitButton.onClick.AddListener(DrawGameLost);
        GameManager.OnGameLost += DrawGameLost;
        GameManager.OnGameWon += DrawGameWon;
    }

    private void Update()
    {
        scoreText.text = $"Score: {GameManager.instance.GetScore()}";
    }

    private void DrawGameLost()
    {
        GameManager.OnGameLost -= DrawGameLost;
        scoreText.gameObject.SetActive(false);
        exitButton.gameObject.SetActive(false);
        gameLostObject.SetActive(true);
        lostScoreText.text = $"Score: {GameManager.instance.GetScore()}";
        lostQuitButton.onClick.AddListener(() => OnGameQuit?.Invoke());
    }

    private void DrawGameWon()
    {
        GameManager.OnGameWon -= DrawGameWon;
        scoreText.gameObject.SetActive(false);
        exitButton.gameObject.SetActive(false);
        gameWonObject.SetActive(true);
        wonScoreText.text = $"Score: {GameManager.instance.GetScore()}";
        wonQuitButton.onClick.AddListener(() => OnGameQuit?.Invoke());
    }
}
