using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Loads and manages the game's levels.
/// </summary>
public class LevelManager : MonoBehaviour
{
    private LevelDataScriptableObject[] levels;

    [HideInInspector]
    public LevelDataScriptableObject curLevel;

    // Instance
    public static LevelManager inst;

    void Awake()
    {
        // Set the instance to this script.
        inst = this;
    }

    void Start()
    {
        GetLevels();
    }

    // Loads in all the level data files.
    void GetLevels()
    {
        levels = Resources.LoadAll<LevelDataScriptableObject>("Levels");

        // In the menu we save the level to play in player prefs when we click on a level button.
        // Let's set that level to be the one to be played.
        int levelToPlay = PlayerPrefs.GetInt("LevelToPlay");
        SetLevel(levelToPlay);
    }

    // Sets the current level.
    public void SetLevel(int levelIndex)
    {
        curLevel = levels[levelIndex];

        // Set the background color.
        Camera.main.backgroundColor = curLevel.cameraBackgroundColor;
    }
}