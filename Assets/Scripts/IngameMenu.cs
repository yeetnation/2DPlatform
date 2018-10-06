using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class IngameMenu : MonoBehaviour
{
    SoundManager soundManager;
    public GameMaster gm;
    public void QuitMenuSelectYes()
    {
        gm.noQuit();
        SceneManager.LoadScene("Menu");
    }
    public void QuitMenuSelectNo()
    {
        GameObject go = GameObject.Find("ReallyQuitMenu");
        gm.noQuit();
    }
}
