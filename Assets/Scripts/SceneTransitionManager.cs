using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneTransitionManager : MonoBehaviour
{
    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
        TitleUIHandler.OnGameStarted += LoadGame;
    }

    private void LoadTitle()
    {
        
    }

    private void LoadGame()
    {
        TitleUIHandler.OnGameStarted -= LoadGame;
        SceneManager.LoadScene("Game");
    }
}
