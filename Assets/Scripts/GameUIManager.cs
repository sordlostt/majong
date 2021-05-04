using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameUIManager : MonoBehaviour
{
    [SerializeField]
    Button exitButton;
    [SerializeField]
    TMP_Text scoreText;

    private void Awake()
    {
        exitButton.onClick.AddListener(GameManager.instance.EndLevel);
    }

    private void Update()
    {
        scoreText.text = GameManager.instance.GetScore().ToString();
    }
}
