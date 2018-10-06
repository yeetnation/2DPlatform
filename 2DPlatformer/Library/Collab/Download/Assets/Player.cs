using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {
    
    [System.Serializable]
    public class PlayerStats
    {
        public int Health = 100;
    }
    public PlayerStats playerStats = new PlayerStats();
    void Update()
    {
        Camera mainCam = Camera.main;
        if (transform.position.y <= mainCam.ScreenToWorldPoint(mainCam.transform.position).y)
        {
            DamagePlayer(Mathf.Infinity);
        }
    }

    public void DamagePlayer(float damage)
    {
        playerStats.Health -= (int)damage;
        if (playerStats.Health <= 0)
        {
            GameMaster.KillPlayer(this);
        }
    }
}
