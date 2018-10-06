using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(EnemyAI))]
public class Enemy : MonoBehaviour
{

    [System.Serializable]
    public class EnemyStats
    {
        public int maxHealth = 100;


        private int _curHealth;
        public int curHealth
        {
            get { return _curHealth; }
            set { _curHealth = Mathf.Clamp(value, 0, maxHealth); }
        }

        public int damage = 40;
        public int moneyWorth = 20;


        public void Init()
        {
            curHealth = maxHealth;
        }
    }
    public EnemyStats stats = new EnemyStats();

    SoundManager soundManager;

    public Transform deathParticles;
    public float shakeAmt = 0.1f;
    public float shakeLength = 0.1f;
    
    private Vector2 previousForce;

    [Header("Optional: ")]
    [SerializeField] private StatusIndicator statusIndicator;

    private void Start()
    {
        stats.Init();
        if (statusIndicator != null)
        {
            statusIndicator.SetHealth(stats.curHealth, stats.maxHealth);
        }
        if (deathParticles == null)
        {
            Debug.LogError("No Deathparticles on Enemy");
        }
        //GameMaster.gm.onTogglePauseGame += OnPauseGameToggle;
        soundManager = SoundManager.instance;
        if (soundManager == null)
            Debug.LogError("No SoundManager found");

    }
    //outdated GamePause... solved with timescale = 0.0f during pause
    void OnPauseGameToggle(bool active)
    {
        
        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        // handle what happens when the PauseMenu is toggled
        if (active == true)
        {
            previousForce = rb.velocity;
            GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeAll;
        }
        else if (active == false)
        {
            GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.None | RigidbodyConstraints2D.FreezeRotation;
            rb.velocity = previousForce;
        }
        GetComponent<EnemyAI>().enabled = !active;
    }
    public void DamageEnemy(float damage)
    {
        stats.curHealth -= (int)damage;
        if (stats.curHealth <= 0)
        {
            GameMaster.KillEnemy(this);
            GameMaster.Money += stats.moneyWorth;
            soundManager.PlaySoundAtPoint(this.gameObject, "EnemyDeathSound");
        }
        if (statusIndicator != null)
        {
            statusIndicator.SetHealth(stats.curHealth, stats.maxHealth);
        }
    }
    void OnCollisionEnter2D(Collision2D collision)
    {
        Player _player = collision.collider.GetComponent<Player>();
        if (_player != null)
        {
            _player.DamagePlayer(stats.damage);
            DamageEnemy(99999);
        }
    }
    private void OnDestroy()
    {
        //GameMaster.gm.onTogglePauseGame -= OnPauseGameToggle;
    }
}
