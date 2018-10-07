using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour {
    
    public KeyCode[] keyInput;
    private static KeyCode defaultKey;
    private KeyCode prevKey;

    // Use this for initialization
    void Start () {
        if (keyInput.Length == 0)
            Debug.LogError("No Key Inputs declared");
    }
	
    /// <summary>
    /// Handle KeyInputs here
    /// Template
    /// </summary>
    /// <param name="keyPressed"></param>
    public void inputHandling(KeyCode keyPressed)
    {
        //Template
        /*
        if (KeyCode.U == keyPressed && LastPressed(keyPressed))
        {
            GameMaster.gm."Function()";
        }
        */
        if (KeyCode.Escape == keyPressed && LastPressed(keyPressed))
        {
            GameMaster.gm.noQuit();
        }
        if (KeyCode.U == keyPressed && LastPressed(keyPressed))
        {
            GameMaster.gm.upgradeMenuOpenClose();
        }
    }

    private bool LastPressed(KeyCode _keyPressed)
    {
        if (prevKey == defaultKey)
        {
            prevKey = _keyPressed;
            return true;
        }
        else if (_keyPressed == prevKey)
        {
            prevKey = defaultKey;
            return true;
        }
        else return false;
    }
}
