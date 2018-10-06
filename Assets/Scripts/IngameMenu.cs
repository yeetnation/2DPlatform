using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class IngameMenu : MonoBehaviour
{
    SoundManager soundManager;
    public void MenuSelect()
	{
        SceneManager.LoadScene("Menu");
    }
}
