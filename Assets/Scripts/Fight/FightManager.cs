using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameManagers;

public class FightManager : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    static public void ToResult(bool isClear)
    {
        MainGameManager.instance.isClear = isClear;
        GameTimeManager.instance.StopTimer();
        SceneFadeManager.instance.FadeOut("Result");
        
    }
}
