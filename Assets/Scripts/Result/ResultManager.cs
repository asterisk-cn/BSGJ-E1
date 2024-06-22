using GameManagers;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResultManager : MonoBehaviour
{
    [SerializeField] private GameObject _clearPanel;
    [SerializeField] private GameObject _gameOverPanel;
    bool select;
    // Start is called before the first frame update
    void Start()
    {
        _clearPanel.SetActive(false);
        _gameOverPanel.SetActive(false);

        if (MainGameManager.instance.isClear)
        {
            _clearPanel.SetActive(true);
        }
        else
        {
            _gameOverPanel.SetActive(true);
        }
        select =false;
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
}
