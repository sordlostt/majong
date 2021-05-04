using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class TitleManager : MonoBehaviour
{
    [SerializeField]
    List<Button> levelButtons = new List<Button>();

    private void Awake()
    { 
        foreach (var button in levelButtons)
        {
            var lvlButton = button.GetComponent<LevelButton>();
            button.GetComponentInChildren<TMP_Text>().text = lvlButton.layout.name;
            button.onClick.AddListener(delegate { LoadGameLevel(lvlButton.layout); });
        }    
    }

    private void LoadGameLevel(TextAsset layout)
    {
        LayoutParser.instance.SetActiveLayout(layout);
        SceneManager.LoadScene("Game");
    }
}
