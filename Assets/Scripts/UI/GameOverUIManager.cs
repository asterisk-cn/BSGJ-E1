using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameOverUIManager : MonoBehaviour
{
    public Sprite[] DishSprites;
    public Image DishImage;
    public string[] DishNames;
    public TextMeshProUGUI DishNameTMP;

    [TextArea (1, 5)] public string Advice0, Advice1, Advice2, Advice3, Advice4;
    string[] Advice = new string[5];
    public TextMeshProUGUI AdviceTMP;
    int nowNum;

    private void Start()
    {
        nowNum = Random.Range(0, DishSprites.Length);
        DishImage.sprite = DishSprites[nowNum];
        DishNameTMP.text = DishNames[nowNum];

        Advice[0] = Advice0;
        Advice[1] = Advice1;
        Advice[2] = Advice2;
        Advice[3] = Advice3;
        Advice[4] = Advice4;
        AdviceTMP.text = Advice[Random.Range(0, 5)];
    }
}
