using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameMaster : MonoBehaviour
{
    public static GameMaster gm;
    [SerializeField] private int maxLives = 3;
    private static int _remainingLives;
    public static int RemainingLives
    {
        get { return _remainingLives; }
    }
    void Awake()
    {
        if (gm == null)
        {
            gm = GameObject.FindGameObjectWithTag("GM").GetComponent<GameMaster>();
        }
    }
    public Transform playerPrefab;
    public Transform spawnPoint;
    public GameObject spawnPrefab;
    public int spawnDelay = 2;
    public CameraShake cameraShake;
    [SerializeField] private GameObject gameOverUI;
    [SerializeField] private GameObject menuButton;
    [SerializeField] private GameObject upgradeMenu;
    public delegate void PauseGame(bool active);
    public PauseGame onTogglePauseGame;
    private SoundManager soundManager;

    private bool keysEnabled = true;

    [SerializeField] private int startingMoney;
    public static int Money;
    private void Start()
    {
        _remainingLives = maxLives;
        Money = startingMoney;
        if (cameraShake == null)
        {
            Debug.LogError("No Deathparticles on Enemy");
        }
        soundManager = SoundManager.instance;
        if (soundManager == null)
            Debug.LogError("No SoundManager found");
        soundManager.ChangeMusic("MusicHappy");
    }
    private void Update()
    {
        if (keysEnabled)
        {
            if (Input.GetKeyDown(KeyCode.U))
            {
                ToggleUpgradeMenu();
            }
        }
    }
    private void ToggleUpgradeMenu()
    {
        upgradeMenu.SetActive(!upgradeMenu.activeSelf);
        GamePause(upgradeMenu.activeSelf);
    }

    private void GamePause(bool pause)
    {
        onTogglePauseGame.Invoke(pause);
        if (pause)
            Time.timeScale = 0.0F;
        else
            Time.timeScale = 1.0f;
    }

    public void EndGame()
    {
        gameOverUI.SetActive(true);
        menuButton.SetActive(false);
    }
    public IEnumerator _RespawnPlayer()
    {
        keysEnabled = false;
        yield return new WaitForSeconds(spawnDelay);
        Instantiate(playerPrefab, spawnPoint.position, spawnPoint.rotation);
        GameObject clone = Instantiate(spawnPrefab, spawnPoint.position, spawnPoint.rotation);
        keysEnabled = true;
        Destroy(clone, 3f);
        soundManager.PlaySound("RespawnSound");

    }

    public static void KillPlayer(Player player)
    {
        Destroy(player.gameObject);
        _remainingLives--;
        if (_remainingLives <= 0)
        {
            gm.EndGame();
        }
        else
        {
            gm.StartCoroutine(gm._RespawnPlayer());
        }
    }
    public static void KillEnemy(Enemy enemy)
    {
        gm._KillEnemy(enemy);
    }
    public void _KillEnemy(Enemy _enemy)
    {
        Instantiate(_enemy.deathParticles, _enemy.transform.position, Quaternion.identity);
        cameraShake.Shake(_enemy.shakeAmt, _enemy.shakeLength);
        Destroy(_enemy.gameObject);
    }

}
