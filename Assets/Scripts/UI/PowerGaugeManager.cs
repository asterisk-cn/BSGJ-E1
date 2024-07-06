using System.Collections;
using System.Collections.Generic;
using Players;
using UnityEngine;
using UnityEngine.UI;
using static UnityEditorInternal.ReorderableList;

public class PowerGaugeManager : MonoBehaviour
{
    public GameObject Fire;
    public Sprite NotMax, Max;
    public Sprite[] GaugeEffectSprites;
    public Image PowerGauge, PowerGaugeFrame, GaugeEffect_Image;
    public float maxPower, PowerUpTime, gaugeShakeAMP;
    public float powerUpNum; //合体成功時の変化量を代入
    public float powerDownNum; //被ダメ時の変化量を代入

    float currentPower, beforePower;
    Coroutine _coroutine;

    [SerializeField] private Players.PlayerCore _playerCore;

    //テスト用
    float testPower;

    private void Start()
    {
        Fire.SetActive(false);
        PowerGauge.fillAmount = 0;
        PowerGaugeFrame.sprite = NotMax;
    }

    private void Update()
    {
        currentPower = _playerCore.GetCurrentUnionCount() / _playerCore.GetTargetUnionCount();
        if (currentPower != beforePower)
        {
            StartChangePowerGaugeCoroutine(currentPower);
            beforePower = currentPower;
        }

        //テスト用
        //if (Input.GetKeyDown(KeyCode.I))
        //{
        //    testPower += 0.2f;
        //    StartCoroutine("ChangePowerGauge", testPower);
        //}
        //if (Input.GetKeyDown(KeyCode.H))
        //{
        //    testPower -= 0.1f;
        //    StartCoroutine("ChangePowerGauge", testPower);
        //}
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
        float interval = 0.02f;
        int times = (int)(PowerUpTime / interval);
        Vector3 defaultPos = transform.position;

        for (i = 0; i < times; i++)
        {
            if (ChangeNum < 0)
            {
                transform.transform.position = defaultPos + Random.insideUnitSphere * gaugeShakeAMP;
            } else if(i == 0)
            {
                StartCoroutine("GaugeEffect");
            }

            PowerGauge.fillAmount += ChangeNum / times;
            yield return new WaitForSecondsRealtime(interval);
        }

        //調整用
        PowerGauge.fillAmount = newPower;
        transform.position = defaultPos;


        //Max時処理
        if (_playerCore.GetCurrentUnionCount() >= _playerCore.GetTargetUnionCount())
        {
            PowerGauge.fillAmount = 1f;
            MaxPowerGauge();
        }
    }

    IEnumerator GaugeEffect()
    {
        int i;

        for (i = 0; i < GaugeEffectSprites.Length; i++)
        {
            GaugeEffect_Image.sprite = GaugeEffectSprites[i];
            yield return new WaitForSecondsRealtime(0.034f);
        }
    }

    void MaxPowerGauge()
    {
        PowerGaugeFrame.sprite = Max;
        Fire.SetActive(true);
        PowerGauge.fillAmount = 1;
    }
}
