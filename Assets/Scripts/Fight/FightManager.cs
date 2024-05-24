using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
        SceneFadeManager.instance.FadeOut("Result");
    }
}
