using GameManagers;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResultManager : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void OnRetryButton()
    {
        SceneFadeManager.instance.FadeOut("Main");
        AudioManager.Instance.PlaySE("Button_SE");
    }

    public void OnTitleButton()
    {
        SceneFadeManager.instance.FadeOut("Title");
        AudioManager.Instance.PlaySE("Button_SE");
    }
}
