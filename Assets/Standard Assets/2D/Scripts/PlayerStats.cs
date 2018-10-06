using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[SerializeField]
public class PlayerStats : MonoBehaviour
{
    public static PlayerStats instance;

    public int maxHealth = 100;
    private int _curHealth;
    public int curHealth
    {
        get { return _curHealth; }
        set { _curHealth = Mathf.Clamp(value, 0, maxHealth); }
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
