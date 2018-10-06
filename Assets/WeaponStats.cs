using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[SerializeField]
public class WeaponStats : MonoBehaviour
{

    public static WeaponStats instance;
    public int pistolDMG = 10;
    public int rifleDMG = 10;
    public float rifleFR = 10;
    private GameObject arm;
    private Weapon pistol;
    private Weapon rifle;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }
    public void SaveStats()
    {
        arm = GameObject.Find("Arm");
        pistol = arm.transform.GetChild(0).GetComponent<Weapon>();
        pistolDMG = pistol.damage;
        rifle = arm.transform.GetChild(1).GetComponent<Weapon>();
        rifleDMG = rifle.damage;
        rifleFR = rifle.fireRate;
    }
}
