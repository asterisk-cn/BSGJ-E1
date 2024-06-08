using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PowerGaugeManager : MonoBehaviour
{
    public Image PowerGauge;
    public float maxPower;
    float testPower;

    private void Start()
    {
    }

    private void Update()
    {
        //テスト用
        //P を押したらパワーアップ
        if (Input.GetKeyDown(KeyCode.P))
        {
            testPower += 2;
            ChangePowerGauge(testPower);
        }
        //H を押したらパワーダウン
        if (Input.GetKeyDown(KeyCode.H))
        {
            testPower -= 1;
            ChangePowerGauge(testPower);
        }
    }

    void ChangePowerGauge(float nowPower)
    {
        float x;
        x = nowPower / maxPower;

        PowerGauge.fillAmount = x;
    }
}
