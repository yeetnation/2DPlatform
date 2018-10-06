using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets._2D;

[RequireComponent(typeof(Platformer2DUserControl))]
public class Player : MonoBehaviour
{
    private bool alive = true;
    private Camera mainCam;

    SoundManager soundManager;

    [SerializeField] private StatusIndicator statusIndicator;

    private PlayerStats stats;
    public Weapon pistol;
    public Weapon rifle;

    private WeaponStats weaponstats;
    private bool keysEnabled = true;
    Vector2 previousForce;

    void Start()
    {
        stats = PlayerStats.instance;
        stats.curHealth = stats.maxHealth;
        if (statusIndicator == null)
            Debug.LogError(this);
        else
        {
            statusIndicator.SetHealth(stats.curHealth, stats.maxHealth);
        }
        //GameMaster.gm.onTogglePauseGame += OnPauseGameToggle;
        soundManager = SoundManager.instance;
        if (soundManager == null)
            Debug.LogError("No SoundManager found");
        weaponstats = WeaponStats.instance;
        if (weaponstats == null)
            Debug.LogError("No WeaponStats found");
        pistol = GameObject.Find("Arm").transform.GetChild(0).GetComponent<Weapon>();
        pistol.damage = weaponstats.pistolDMG;
        rifle = GameObject.Find("Arm").transform.GetChild(1).GetComponent<Weapon>();
        rifle.damage = weaponstats.rifleDMG;
        rifle.fireRate = weaponstats.rifleFR;
        InvokeRepeating("RegenHealth", 1f, 0.5f);
    }

    void Update()
    {
        mainCam = Camera.main;
        if (transform.position.y <= mainCam.ScreenToWorldPoint(mainCam.transform.position).y)
        {
            DamagePlayer(Mathf.Infinity);
        }
        if(keysEnabled)
        {
            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                WeaponSwitch(1);
            }
            if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                WeaponSwitch(2);
            }
        }
        
    }
    void RegenHealth()
    {
        stats.curHealth += (stats.healthRegenRate / 2);

        statusIndicator.SetHealth(stats.curHealth, stats.maxHealth);
    }
    //outdated GamePause... solved with timescale = 0.0f during pause
    void OnPauseGameToggle(bool active)
    {
        // handle what happens when the upgrade menu is toggled
        if (this == null)
            return;
        keysEnabled = !active;
        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        GetComponent<Platformer2DUserControl>().enabled = !active;
        if (active == true)
        {
            previousForce = rb.velocity;
            rb.constraints = RigidbodyConstraints2D.FreezeAll;
            CancelInvoke("RegenHealth");
        }
        else if (active == false)
        {
            rb.constraints = RigidbodyConstraints2D.None | RigidbodyConstraints2D.FreezeRotation;
            rb.velocity = previousForce;
            InvokeRepeating("RegenHealth", 1f, 0.5f);
        }
        Weapon _weapon = GetComponentInChildren<Weapon>();
        ArmRotation _armRotation = GetComponentInChildren<ArmRotation>();
        if (_weapon != null)
        {
            _weapon.enabled = !active;
        }
        if (_armRotation != null)
        {
            _armRotation.enabled = !active;
        }
    }
    public void DamagePlayer(float damage)
    {
        stats.curHealth -= (int)damage;

        soundManager.PlaySound("PlayerHitSound");
        if (stats.curHealth <= 0 && alive == true)
        {
            weaponstats.SaveStats();
            alive = false;
            GameMaster.KillPlayer(this);

            soundManager.PlaySound("PlayerDeathSound");
        }
        if (statusIndicator != null)
        {
            statusIndicator.SetHealth(stats.curHealth, stats.maxHealth);
        }

    }
    private void WeaponSwitch(int _weaponSlot)
    {
        if (_weaponSlot == 1)
        {
            pistol.gameObject.SetActive(true);
            rifle.gameObject.SetActive(false);
        }
        else if (_weaponSlot == 2)
        {
            pistol.gameObject.SetActive(false);
            rifle.gameObject.SetActive(true);
        }
    }
    private void OnDestroy()
    {
        //GameMaster.gm.onTogglePauseGame -= OnPauseGameToggle;
    }
}
