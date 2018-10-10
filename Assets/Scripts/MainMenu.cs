using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour {
    SoundManager soundManager;

    public void Start()
    {
        soundManager = SoundManager.instance;
        if (soundManager == null)
            Debug.LogError("No SoundManager found");

        soundManager.LoadAudioSettings();
        soundManager.ChangeMusic("MusicMenu");
        
        
    }
    // Use this for initialization
    public void PlayGame()
    {
        SceneManager.LoadScene("Game");
    }
    public void QuitGame()
    {
        Application.Quit();
    }
    public void OnMouseOver()
    {
        soundManager.PlaySound("ButtonHover");
    }
    public void OnMouseDown()
    {
        soundManager.PlaySound("ButtonPress");
    }
}
