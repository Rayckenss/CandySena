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
    public GameObject levelsMenu;

    
    private void Start()
    {
        levelsMenu.SetActive(false);
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
        levelsMenu.SetActive(true);
    }
    public void ExitButton()
    {
        Application.Quit();
    }
    public void MenuSelection(int seleccion)
    {
        switch (seleccion)
        {
            case 1:
                SceneManager.LoadScene(2);
                break;
            case 2:
                SceneManager.LoadScene(3);
                break;
            case 3:
                SceneManager.LoadScene(4);
                break;
            case 4:
                SceneManager.LoadScene(5);
                break;
            case 5:
                SceneManager.LoadScene(6);
                break;
            case 0:
                SceneManager.LoadScene(1);
                break;
            default:
                break;
        }
    }
    
}
