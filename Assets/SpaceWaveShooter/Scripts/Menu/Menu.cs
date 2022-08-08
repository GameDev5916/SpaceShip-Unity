using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Menu : MonoBehaviour
{
    [Header("Screens")]
    public GameObject mainScreen;
    public GameObject playScreen;
    public GameObject settingsScreen;

    private void Start()
    {
        mainScreen.SetActive(true);
    }
    // Sets the currently visible screen.
    public void SetScreen (GameObject screen)
    {
        // Disable all screens.
        mainScreen.SetActive(false);
        playScreen.SetActive(false);
        settingsScreen.SetActive(false);

        // Enable the requested screen.
        screen.SetActive(true);
    }

    // Called when we press the 'Quit' button.
    public void OnQuitButton ()
    {
        Application.Quit();
    }
}