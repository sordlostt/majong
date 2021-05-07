using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneTransitionManager : Singleton<SceneTransitionManager>
{
    protected override void Awake()
    {
        base.Awake();
        DontDestroyOnLoad(this);
    }

    public void LoadTitle()
    {
        SceneManager.LoadScene("Title");
    }

    public void LoadGame()
    {
        SceneManager.LoadScene("Game");
    }
}
