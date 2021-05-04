using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class GameUIHandler : MonoBehaviour
{
    [SerializeField]
    Button exitButton;
    [SerializeField]
    TMP_Text scoreText;

    public static event Action OnGameQuit;

    private void Awake()
    {
        exitButton.onClick.AddListener(() => OnGameQuit?.Invoke());
    }

    private void Update()
    {
        scoreText.text = $"Score: {GameManager.instance.GetScore()}";
    }
}
