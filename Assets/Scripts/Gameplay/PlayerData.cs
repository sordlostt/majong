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

    public float HighScore
    {
        get
        {
            if (PlayerPrefs.HasKey("Highscore"))
            {
                return PlayerPrefs.GetFloat("Highscore");
            }
            else
            {
                return 0;
            }
        }
    }

    public void RefreshHighScore(float score)
    {
        if (score > HighScore)
        {
            PlayerPrefs.SetFloat("Highscore", score);
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
