using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealthPanelManager : MonoBehaviour
{
    public Sprite HealthSprite, HealthBreakSprite;

    public Image[] HealthImages;
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

        testHealth = _playerCore.GetCurrentHealth();
        ChangeHealth(testHealth);
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
    }
}
