using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

[SerializeField]

public class PlayerStats : MonoBehaviour
{
    public static PlayerStats instance;
    public int maxHealth = 100;
    private int _curHealth = 0;
    public int curHealth
    {
        get { return _curHealth; }
        set { _curHealth = Mathf.Clamp(value, 0, maxHealth); }
    }
    public int neededExperience = 90;

    private int _curExperience=0;
    public int curExperience
    {
        get { return _curExperience;}
        set
        {
            _curExperience = value;
            
            if (curExperience >= neededExperience)
            {
                _curExperience = _curExperience - neededExperience;
            }
        }
    }
    

    private int _curLvl = 1;
    public int curlvl
    {
        get { return _curLvl; }
        set { _curLvl = value; }
    }

    public int healthRegenRate = 2;

    public float movementSpeed = 10f;
    public float jumpForce = 400f;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        
    }


}
