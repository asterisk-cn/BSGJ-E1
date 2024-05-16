using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Timer : MonoBehaviour
{
    private TextMeshProUGUI _timerText;
    // Start is called before the first frame update
    void Start()
    {
        _timerText = GetComponent<TextMeshProUGUI>();
    }

    // Update is called once per frame
    void Update()
    {
        var seconds = GameManagers.GameTimeManager.instance.GetTimeSecondsLeft();
        _timerText.text = $"{seconds / 60:0}:{seconds % 60:00}";
    }
}
