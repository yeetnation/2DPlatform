using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerInfoUpdateUI : MonoBehaviour {

    [SerializeField] private Text moneyText;
    [SerializeField] private Text livesText;
    [SerializeField] private Text lvlText;

    public void PlayerInfoUpdate() {
        moneyText.text = "Money: " + GameMaster.Money;
        livesText.text = "Lives: " + GameMaster.RemainingLives;
        lvlText.text = "Level: " + PlayerStats.instance.curlvl;
    }
}
