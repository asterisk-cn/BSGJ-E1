using GameManagers;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ResultManager : MonoBehaviour
{
    [SerializeField] private GameObject _clearPanel;
    [SerializeField] private GameObject _gameOverPanel;

    [SerializeField] private TextMeshProUGUI _scoreText;

    bool select = false;

    // Start is called before the first frame update
    void Start()
    {
        _clearPanel.SetActive(false);
        _gameOverPanel.SetActive(false);

        if (MainGameManager.instance.isClear)
        {
            string scoreRank = MainGameManager.instance.GetScoreRank();
            string clearTime = TimeToString(MainGameManager.instance.GetClearTime());
            string soulDeadCount = MainGameManager.instance.GetSoulDeadCount().ToString();

            _clearPanel.SetActive(true);
            _scoreText.text = "Result " + scoreRank + "\n" + "ClearTime " + clearTime + "\n" + "Dead Souls " + soulDeadCount;
        }
        else
        {
            _gameOverPanel.SetActive(true);
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void OnRetryButton()
    {
        SceneFadeManager.instance.FadeOut("Main");
        if(!select)AudioManager.Instance.PlaySE("Button_SE");
        select =true;
    }

    public void OnTitleButton()
    {
        SceneFadeManager.instance.FadeOut("Title");
       if(!select) AudioManager.Instance.PlaySE("Button_SE");
       select =true;
    }

    private string TimeToString(float time)
    {
        int minute = (int)time / 60;
        int second = (int)time % 60;

        return minute.ToString() + ":" + second.ToString("00");
    }
}
