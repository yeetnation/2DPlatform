using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Text))]
public class LivesCounterUI : MonoBehaviour {

    private Text livesText;
    // Use this for initialization
    private void Awake()
    {
        livesText = GetComponent<Text>();
    }

    // Update is called once per frame
    void Update () {
        livesText.text = "Lives: " + GameMaster.RemainingLives;
	}
}
