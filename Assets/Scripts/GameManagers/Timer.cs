using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Timer : MonoBehaviour
{
    [SerializeField]
    TextMeshProUGUI text;

    [SerializeField]
    [Header("��")]
    int minutes;
    [SerializeField]
    [Header("�b")]
    float seconds = 0;



    SceneFadeManager sceneFadeManager;
    // Start is called before the first frame update
    void Start()
    {
        text = GetComponent<TextMeshProUGUI>();
        sceneFadeManager = SceneFadeManager.instance.GetComponent<SceneFadeManager>();
    }

    // Update is called once per frame
    void Update()
    {
        CountTime();
    }

    void CountTime()
    {
        if (seconds < 0)
        {
            minutes--;
            seconds = 60;
        }
        seconds -= Time.deltaTime;
        text.text = minutes.ToString() + ":" + seconds.ToString("00");

        if (seconds <= 0 && minutes <= 0)
        {
            seconds = 0;
            minutes = 0;
            MainGameManager.instance.gameState = GameManagers.GameState.Fight;
            SceneFadeManager.instance.FadeOut(SceneNameClass.SceneName.Main);
        }
    }
}
