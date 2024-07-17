using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FightUIManager : MonoBehaviour
{
    public Image SheffFace_Image;
    public Sprite SheffFaceBatsuIcon;
    public GameObject SheffHat_Obj, Arm_Obj, Fire_Obj;
    Transform SheffHat_Trans, Arm_Trans, Fire_Trans;
    int sheffHP, sheffMaxHP, attackCounter;
    bool first;
    float dt, fireCount, time, pushRate, backRate;
    public Vector3 DefaultSheffHatSize, DiffFromHatToArm;

    public float fightTime, imageRate, pushPow, repushPow, fireTime;
    int nowHP;

    [SerializeField] private Enemy.EnemyCore _enemyCore;

    private void Start()
    {
        SheffHat_Trans = SheffHat_Obj.GetComponent<Transform>();
        Arm_Trans = Arm_Obj.GetComponent<Transform>();
        Fire_Trans = Fire_Obj.GetComponent<Transform>();
        DefaultSheffHatSize = SheffHat_Trans.localScale;
        DiffFromHatToArm = Arm_Trans.position - SheffHat_Trans.position;
        backRate = repushPow / 2f;

        first = true; //終了判定用

        time = fightTime; //ファイトシーンの制限時間
        Fire_Obj.SetActive(false);
    }

    private void Update()
    {
        sheffHP = _enemyCore.GetCurrentHealth();

        if(sheffHP > 0)
        {
            if (nowHP == 0) //初回
                sheffMaxHP = sheffHP;

            dt = Time.deltaTime;
            if (time > 0)
            {
                time -= dt;
                fireCount += dt;

                //押し返されている割合が大きいほど押し返す力を下げる
                //残り時間が少ないほど押し返す力を上げる
                pushRate = backRate * (1 - imageRate) / (time / fightTime);

                imageRate += backRate * pushRate * dt / fightTime;

                if (sheffHP != nowHP)
                {
                    imageRate -= pushPow * imageRate; //パンチで体力が減ったら押す
                    Fire_Obj.SetActive(true);
                    fireCount = 0;
                    attackCounter++;
                    if(attackCounter >= 10) //攻撃10回ごとに押し返す力を変更
                    {
                        attackCounter = 0;
                        backRate = repushPow * Random.Range(0f, (float)sheffHP / (float)sheffMaxHP); //残り体力が低いほど押し返す力の乱数範囲を下げる
                    }
                }
                nowHP = sheffHP;
                UpdateUI(imageRate);

                if (fireCount > fireTime)
                {
                    Fire_Obj.SetActive(false);
                }

                ////テスト用
                //if (Input.GetMouseButtonDown(0))
                //{
                //    sheffHP -= 1;
                //}
                //if (Input.GetMouseButtonDown(1))
                //{
                //    sheffHP += 10;
                //}
            } else if(first)
            {
                first = false;
                Fire_Obj.SetActive(false);
                StartCoroutine("GameOverAnimation", imageRate);
            }
        }
        else if(first)
        {
            first = false;
            Fire_Obj.SetActive(true);
            StartCoroutine("ClearAnimation", imageRate);
        }
    }

    void UpdateUI(float rate)
    {
        if (rate > 0)
        {
            SheffHat_Trans.localScale = new Vector3(rate * DefaultSheffHatSize.x, DefaultSheffHatSize.y, DefaultSheffHatSize.z);
            float dist = (SheffHat_Trans.position.x + rate * DiffFromHatToArm.x) - Arm_Trans.position.x;
            Arm_Trans.Translate(dist, 0, 0);
            Fire_Trans.Translate(0, -dist, 0);
        }
    }

    IEnumerator ClearAnimation(float rate)
    {
        float interval = 0.01f;
        float times = 0.5f / interval;
        float stepRate = rate / times;
        float dist;
        int i;

        for(i= 0; i < times; i++)
        {
            if(rate > 0)
            {
                rate -= stepRate;

                SheffHat_Trans.localScale = new Vector3(rate * DefaultSheffHatSize.x, DefaultSheffHatSize.y, DefaultSheffHatSize.z);
                dist = (SheffHat_Trans.position.x + rate * DiffFromHatToArm.x) - Arm_Trans.position.x;
                Arm_Trans.Translate(dist, 0, 0);
                Fire_Trans.Translate(0, -dist, 0);
                yield return new WaitForSecondsRealtime(interval);
            }
        }

        SheffFace_Image.sprite = SheffFaceBatsuIcon;
        SheffHat_Obj.SetActive(false);
    }

    IEnumerator GameOverAnimation(float rate)
    {
        float interval = 0.01f;
        float times = 0.5f / interval;
        float stepRate = 1.5f / times;
        float dist;
        int i;

        for (i = 0; i < times; i++)
        {
            if (rate > 0)
            {
                rate += stepRate;

                SheffHat_Trans.localScale = new Vector3(rate * DefaultSheffHatSize.x, DefaultSheffHatSize.y, DefaultSheffHatSize.z);
                dist = (SheffHat_Trans.position.x + rate * DiffFromHatToArm.x) - Arm_Trans.position.x;
                Arm_Trans.Translate(dist, 0, 0);
                yield return new WaitForSecondsRealtime(interval);
            }
        }
    }
}
