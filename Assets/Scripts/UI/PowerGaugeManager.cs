using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PowerGaugeManager : MonoBehaviour
{
    public GameObject Fire;
    public Sprite NotMax, Max;
    public Image PowerGauge, PowerGaugeFrame;
    public float maxPower, PowerUpTime;
    public float powerUpNum; //合体成功時の変化量を代入
    public float powerDownNum; //被ダメ時の変化量を代入

    private void Start()
    {
        Fire.SetActive(false);
        PowerGauge.fillAmount = 0;
        PowerGaugeFrame.sprite = NotMax;

        //仮値使用中
        powerUpNum = 2f; 
        powerDownNum = -1f; 
    }

    private void Update()
    {
        //テスト用
        //P を押したらパワーアップ
        if (Input.GetKeyDown(KeyCode.P))
        {
            StartCoroutine("ChangePowerGauge", powerUpNum); //パワー変化量を代入
        }
        //H を押したらパワーダウン
        if (Input.GetKeyDown(KeyCode.H))
        {
            StartCoroutine("ChangePowerGauge", powerDownNum); //パワー変化量を代入
        }

        if (PowerGauge.fillAmount >= 1)
        {
            MaxPowerGauge();
        }
    }

    IEnumerator ChangePowerGauge(float ChangeNum)
    {
        int i;
        float a = 100;
        for (i = 0; i < a; i++)
        {
            PowerGauge.fillAmount += ChangeNum / a / maxPower;
            yield return new WaitForSeconds(PowerUpTime / a);
        }

        if(PowerGauge.fillAmount >= 0.999) //誤差調整用
        {
            PowerGauge.fillAmount = 1;
        }
    }

    void MaxPowerGauge()
    {
        PowerGaugeFrame.sprite = Max;
        Fire.SetActive(true);
    }
}
