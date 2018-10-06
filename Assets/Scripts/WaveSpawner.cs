using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveSpawner : MonoBehaviour
{
    public enum SpawnState { Spawning, Waiting, Counting };
    [System.Serializable]
    public class Wave
    {
        public string name;
        public Transform enemy;
        public int amount;
        public float spawnRate;
    }

    public Wave waves = new Wave();
    private int nextWave = 0;
    public int NextWave
    {
        get { return nextWave + 1; }
    }
    
    public Transform[] spawnPoints;
    public float timeBetweenWaves = 5f;
    private float waveCountdown;
    public float WaveCountdown
    {
        get { return waveCountdown; }
    }

    private float searchCountdown = 1f;
    private bool isPaused = false;

    private SpawnState waveState = SpawnState.Counting;
    public SpawnState State
    {
        get { return waveState; }
    }
    private void Start()
    {
        //GameMaster.gm.onTogglePauseGame += OnPauseGameToggle;
        waveCountdown = 5f;
        if (spawnPoints.Length == 0)
            Debug.LogError("No spawn points referenced");
    }
    //outdated GamePause... solved with timescale = 0.0f during pause
    void OnPauseGameToggle(bool active)
    {
        // handle what happens when the upgrade menu is toggled
        if (this == null)
            return;
        isPaused = !isPaused;
        GetComponent<WaveSpawner>().enabled = !active;
    }
    private void Update()
    {
        if (waveState == SpawnState.Waiting)
        {
            if (!EnemyIsAlive())
            {
                WaveCompleted();
            }
            else
            {
                return;
            }
        }
        if (waveCountdown <= 0)
        {
            if (waveState != SpawnState.Spawning)
            {
                StartCoroutine(SpawnWave(waves));
            }
        }
        else
        {
            waveCountdown -= Time.deltaTime;
        }
    }

    bool EnemyIsAlive()
    {
        searchCountdown -= Time.deltaTime;
        if (searchCountdown <= 0f)
        {
            searchCountdown = 1f;
            if (GameObject.FindGameObjectsWithTag("Enemy").Length == 0)
            {
                return false;
            }
        }
        return true;
    }

    IEnumerator SpawnWave(Wave _wave)
    {
        Debug.Log("Spawning Wave: " + _wave.name);
        waveState = SpawnState.Spawning;

        for (int i = 0; i < _wave.amount; i++)
        {
            while (isPaused)
            {
                yield return null;
            }
            SpawnEnemy(_wave.enemy);
            yield return new WaitForSeconds(1f / _wave.spawnRate);
        }

        waveState = SpawnState.Waiting;

        yield break;
    }

    private void WaveCompleted()
    {
        Debug.Log("Wave Completed");
        ChangeWave();
        nextWave++;
        waveState = SpawnState.Counting;
        waveCountdown = timeBetweenWaves;
    }
    private void ChangeWave()
    {
        Wave prevWave = waves;
        prevWave.amount += 3;
        prevWave.name = "Enemy Invasion: " + (nextWave + 2);
        prevWave.spawnRate += 0.5f;
        waves = prevWave;
    }
    private void SpawnEnemy(Transform _enemy)
    {
        Debug.Log("Spawning enemy: " + _enemy.name);
        Transform _sp = spawnPoints[Random.Range(0, spawnPoints.Length)];
        Instantiate(_enemy, _sp.position, _sp.rotation);
    }
}
