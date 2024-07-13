using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class OPTextManager : MonoBehaviour
{
    public Image TextBackGround;
    public TextMeshProUGUI OPText;
    [TextArea (1, 2)]public string[] strings;
    int textNum;
    bool doneAddText;

    private void Start()
    {
        StartOPText();
    }

    private void Update()
    {
        //Aボタンの入力検知に変更する
        if (Input.anyKeyDown && doneAddText)
        {
            if (textNum < strings.Length)
            {
                StartCoroutine("AddText");
            }
            else //全ての文章を表示しているならメインシーンへ
            {
            }
        }
    }

    //動画再生終了時動画を止めてこれを呼ぶ
    public void StartOPText()
    {
        TextBackGround.color = Color.clear;
        textNum = 0;
        doneAddText = true;
        OPText.text = "";

        StartCoroutine("SetTextBackGround");
    }

    IEnumerator SetTextBackGround()
    {
        int i;
        float interval = 0.01f;
        float time = 0.5f;
        float times = time / interval;

        for (i = 0; i < times; i++)
        {
            TextBackGround.color = new Color(1, 1, 1, i / times);
            yield return new WaitForSecondsRealtime(interval);
        }
        TextBackGround.color = Color.white;

        StartCoroutine("AddText");
    }

    IEnumerator AddText() //テキスト送り
    {
        doneAddText = false;

        int i;
        float interval = 0.02f;

        for (i = 1; i <= strings[textNum].Length; i++)
        {
            OPText.text = strings[textNum].Substring(0, i);
            yield return new WaitForSecondsRealtime(interval);
        }
        textNum++;

        //クリック判定をオンにする
        doneAddText = true;
    }
}
