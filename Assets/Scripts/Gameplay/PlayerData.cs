using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerData : Singleton<PlayerData>
{
    protected override void Awake()
    {
        base.Awake();
        DontDestroyOnLoad(this);
    }

    public float GetHighScore(string levelName)
    {
        if (PlayerPrefs.HasKey("Highscore" + levelName))
        {
            return PlayerPrefs.GetFloat("Highscore" + levelName);
        }
        else
        {
            return 0.0f;
        }
    }

    public void RefreshHighScore(float score, string levelName)
    {
        if (score > GetHighScore(levelName))
        {
            PlayerPrefs.SetFloat("Highscore" + levelName, score);
        }
    }

    public void SetLevelCompleted(string name)
    {
        PlayerPrefs.SetString(name, "completed");
    }

    public bool CheckLevelCompletion(string name)
    {
        if (PlayerPrefs.HasKey(name))
        {
            return true;
        }

        return false;
    }
}
