using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayScreen : MenuScreen
{
    public Button[] levelButtons;
    private LevelDataScriptableObject[] levels;


    void Start ()
    {
        levels = Resources.LoadAll<LevelDataScriptableObject>("Levels");
        // Load in the levels.
        PlayerPrefs.SetInt("LevelToPlay", 0);
        SceneManager.LoadScene("Game");
//      InitializeLevelButtons();
    }

    // Initialize the level buttons, text and OnClick event.
    //void InitializeLevelButtons ()
    //{
    //    for(int x = 0; x < levelButtons.Length; ++x)
    //    {
    //        if(x < levels.Length)
    //            levelButtons[x].gameObject.SetActive(true);
    //        else
    //        {
    //            levelButtons[x].gameObject.SetActive(false);
    //            continue;
    //        }

    //        levelButtons[x].transform.Find("Text").GetComponent<Text>().text = "Level " + (x + 1).ToString();

    //        int levelIndex = x;
    //        levelButtons[x].onClick.AddListener(() => { OnPressLevelButton(levelIndex); });

    //        // Set button color.
    //            //switch(levels[x].difficulty)
    //            //{
    //            //    case LevelDifficulty.Easy:
    //            //        SetButtonColor(levelButtons[x], easyColor);
    //            //        break;
    //            //    case LevelDifficulty.Medium:
    //            //        SetButtonColor(levelButtons[x], mediumColor);
    //            //        break;
    //            //    case LevelDifficulty.Hard:
    //            //        SetButtonColor(levelButtons[x], hardColor);
    //            //        break;
    //            //}
    //    }
    //}

    //void SetButtonColor (Button button, Color color)
    //{
    //    ColorBlock cb = button.colors;

    //    cb.normalColor = color;
    //    cb.highlightedColor = color;
    //    cb.pressedColor = color;

    //    button.colors = cb;
    //}

    // Called when the player presses a level button.
    //void OnPressLevelButton (int levelIndex)
    //{
    //    // Set the level to play in our player prefs and load the game scene.
    //    PlayerPrefs.SetInt("LevelToPlay", levelIndex);
    //    SceneManager.LoadScene("Game");
    //}
}