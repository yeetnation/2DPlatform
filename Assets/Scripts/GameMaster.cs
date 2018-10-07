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

    internal void upgradeMenuOpenClose()
    {
        upgradeMenu.SetActive(!upgradeMenu.activeSelf);
        GamePause(upgradeMenu.activeSelf);
    }

    [SerializeField] private GameObject gameOverUI;
    [SerializeField] private GameObject menuButton;
    [SerializeField] private GameObject upgradeMenu;
    [SerializeField] private GameObject reallyQuitMenu;
    public delegate void PauseGame(bool active);
    public PauseGame onTogglePauseGame;
    private SoundManager soundManager;
    public InputManager inputManager;

    private bool disableUpgradeMenu = false;
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
        //InputManager
        if(keysEnabled)
        {
            for (int a = 0; a < inputManager.keyInput.Length; a++)
            {
                if (Input.GetKeyDown(inputManager.keyInput[a]))
                    inputManager.inputHandling(inputManager.keyInput[a]);

            }
        }
    }

    public void noQuit()
    {
        //if (keysEnabled)
        //    resetScreen();
        reallyQuitMenu.SetActive(!reallyQuitMenu.activeSelf);
        GamePause(reallyQuitMenu.activeSelf);
        /*if (reallyQuitMenu.activeSelf)
            keysEnabled = false;
        else
            keysEnabled = true;
            */
    }

    public void GamePause(bool pauseStatus)
    {
        onTogglePauseGame.Invoke(pauseStatus);
        if (pauseStatus)
            Time.timeScale = 0.0F;
        else
            Time.timeScale = 1.0f;
    }

    public void EndGame()
    {
        keysEnabled = false;
        gameOverUI.SetActive(true);
        menuButton.SetActive(false);
    }
    public IEnumerator _RespawnPlayer()
    {
        keysEnabled = false;
        disableUpgradeMenu = true;
        yield return new WaitForSeconds(spawnDelay);
        Instantiate(playerPrefab, spawnPoint.position, spawnPoint.rotation);
        GameObject clone = Instantiate(spawnPrefab, spawnPoint.position, spawnPoint.rotation);
        disableUpgradeMenu = false;
        Destroy(clone, 3f);
        soundManager.PlaySound("RespawnSound");
        keysEnabled = true;

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
