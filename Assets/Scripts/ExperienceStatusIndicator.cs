using UnityEngine;
using UnityEngine.UI;

public class ExperienceStatusIndicator : MonoBehaviour {

    [SerializeField] private RectTransform expBarRect;
    [SerializeField] private Text expBarText;
    // Use this for initialization
    void Start ()
    {
        if (expBarRect == null)
        {
            Debug.LogError("STATUS INDICATOR: No experience bar object referenced");
        }
        if (expBarText == null)
        {
            Debug.LogError("STATUS INDICATOR: No experience text object referenced");
        }
    }
    
    public void SetExperience(int _cur, int _max)
    {
        float _value = (float)_cur / _max;
        expBarRect.localScale = new Vector3(_value, expBarRect.localScale.y, expBarRect.localScale.z);
        expBarText.text = _cur + "/" + _max;
    }
}
