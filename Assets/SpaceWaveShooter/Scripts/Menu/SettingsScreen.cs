using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingsScreen : MenuScreen
{
    public Slider volumeSlider;

    void Start ()
    {
        if(!PlayerPrefs.HasKey("Volume"))
            PlayerPrefs.SetFloat("Volume", 1.0f);

        volumeSlider.value = PlayerPrefs.GetFloat("Volume");
    }

    public void OnUpdateVolumeSlider ()
    {
        PlayerPrefs.SetFloat("Volume", volumeSlider.value);
        AudioListener.volume = volumeSlider.value;
    }
}