using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public Image sound;
    public Sprite soundOn, soundOff;

    
    private void Start()
    {
        sound.sprite = OptionManager.music ? soundOff : soundOn;
        GetComponent<AudioSource>().mute = OptionManager.music;
    }
    public void SoundButton()
    {
        OptionManager.music = !OptionManager.music;
        sound.sprite = OptionManager.music ? soundOff : soundOn;
        GetComponent<AudioSource>().mute = OptionManager.music;
    }
    public void PlayButton()
    {
        SceneManager.LoadScene(1);
    }
    public void ExitButton()
    {
        Application.Quit();
    }
}
