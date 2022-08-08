using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class EndGameScreenUI : MonoBehaviour
{
    public GameObject endGameScreen;
    public Text headerText;
    public Text contentsText;

    // Instance
    public static EndGameScreenUI inst;

    void Awake ()
    {
        // Set the instance to this script.
        inst = this;
    }

    // Called when the player either wins or loses the game.
    public void SetEndGameScreen (bool won, int wavesCompleted)
    {
        Time.timeScale = 0.0f;
        endGameScreen.SetActive(true);

        headerText.text = won ? "You Win" : "Game Over";
        headerText.color = won ? Color.green : Color.red;

        contentsText.text = "You survived " + wavesCompleted + " waves!";
    }

    // Called when the "Restart" button is pressed.
    public void OnRestartButton ()
    {
        Time.timeScale = 1.0f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    // Called when the "Menu" button is pressed.
    public void OnMenuButton ()
    {
        Time.timeScale = 1.0f;
        SceneManager.LoadScene(0);
    }
}