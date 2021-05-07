using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class TitleUIHandler : MonoBehaviour
{
    [SerializeField]
    List<Button> levelButtons = new List<Button>();

    public static event Action<TextAsset> OnLayoutPicked;
    public static event Action OnGameStartCalled;

    private void Awake()
    { 
        foreach (Button button in levelButtons)
        {
            LevelButton lvlButton = button.GetComponent<LevelButton>();

            lvlButton.LevelStar.gameObject.SetActive(PlayerData.instance.CheckLevelCompletion(lvlButton.Layout.name));
            button.GetComponentInChildren<TMP_Text>().text = lvlButton.Layout.name;
            button.onClick.AddListener(() => {
                OnLayoutPicked?.Invoke(lvlButton.Layout);
                OnGameStartCalled?.Invoke();
                SceneTransitionManager.instance.LoadGame();
            });
        }    
    }
}
