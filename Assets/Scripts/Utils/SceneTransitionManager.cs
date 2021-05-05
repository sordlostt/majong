using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneTransitionManager : MonoBehaviour
{
    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
        TitleUIHandler.OnGameStartCalled += LoadGame;
    }

    private void LoadTitle()
    {
        
    }

    private void LoadGame()
    {
        TitleUIHandler.OnGameStartCalled -= LoadGame;
        SceneManager.LoadScene("Game");
    }
}
