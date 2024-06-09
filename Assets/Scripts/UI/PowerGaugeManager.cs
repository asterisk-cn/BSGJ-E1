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

    float currentPower;
    Coroutine _coroutine;

    [SerializeField] private Players.PlayerCore _playerCore;

    private void Start()
    {
        Fire.SetActive(false);
        PowerGauge.fillAmount = 0;
        PowerGaugeFrame.sprite = NotMax;
    }

    private void Update()
    {
        if (PowerGauge.fillAmount >= 1)
        {
            MaxPowerGauge();
        }

        currentPower = _playerCore.GetCurrentUnionCount() / _playerCore.GetTargetUnionCount();
        StartChangePowerGaugeCoroutine(currentPower);
    }

    void StartChangePowerGaugeCoroutine(float newPower)
    {
        if (_coroutine != null)
        {
            StopCoroutine(_coroutine);
        }
        _coroutine = StartCoroutine(ChangePowerGauge(newPower));
    }

    IEnumerator ChangePowerGauge(float newPower)
    {
        float nowPower = PowerGauge.fillAmount;
        float ChangeNum = newPower - nowPower;
        int i;
        float a = 100;
        for (i = 0; i < a; i++)
        {
            PowerGauge.fillAmount += ChangeNum / a;
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
        PowerGauge.fillAmount = 1;
    }
}
