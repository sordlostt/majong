using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelButton : MonoBehaviour
{
    [SerializeField]
    private TextAsset layout;
    [SerializeField]
    private Image levelStar;
    public TextAsset Layout { get => layout; }
    public Image LevelStar { get => levelStar; }
}
