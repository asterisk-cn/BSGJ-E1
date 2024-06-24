using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealthPanelManager : MonoBehaviour
{
    public Sprite HealthSprite, HealthBreakSprite;

    public GameObject[] HealthObjects;
    public Image[] HealthImages;
    public float ShakeAMP;
    public int shakeTime; //被ダメージアニメーション時間とそろえる
    int testHealth;
    int maxHealth;

    [SerializeField] private Players.PlayerCore _playerCore;

    private void Start()
    {
        maxHealth = HealthImages.Length;
        testHealth = maxHealth;
    }

    void Update()
    {
        //テスト用
        //H を押したらダメージ
        if (Input.GetKeyDown(KeyCode.H))
        {
            testHealth -= 1;
            ChangeHealth(testHealth);
        }

        if (testHealth != _playerCore.GetCurrentHealth())
        {
            testHealth = _playerCore.GetCurrentHealth();
            ChangeHealth(testHealth);
        }
    }

    public void ChangeHealth(int playerHealth)
    {
        int i;
        for (i = 0; i < maxHealth; i++)
        {
            if(i < playerHealth)
            {
                HealthImages[i].sprite = HealthSprite;
            } else
            {
                HealthImages[i].sprite = HealthBreakSprite;
            }
        }

        StartCoroutine("HitDamage", playerHealth);
    }

    IEnumerator HitDamage(int num)
    {
        Vector3 defaultPos = HealthObjects[num].transform.position;
        for (int i = 0; i < shakeTime * 10; i++)
        {
            HealthObjects[num].transform.position = defaultPos + Random.insideUnitSphere * ShakeAMP;
            yield return new WaitForSeconds(0.1f);
        }
        HealthObjects[num].transform.position = defaultPos;
    }
}
