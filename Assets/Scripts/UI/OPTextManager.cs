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
    int textNum, changeMovie2Num = 1;
    bool doneAddText;

    private void Start()
    {
        //StartOPText1();
    }

    private void Update()
    {
        //Aボタンの入力検知に変更する
        if (Input.anyKeyDown && doneAddText)
        {
            doneAddText = false;
            if (textNum == changeMovie2Num) //動画２へ切り替えるテキストまで進んだら動画２を再生
            {
                Debug.Log("movie2");
                StartOPText2();
            }
            else if(textNum >= strings.Length) //全ての文章を表示しているならメインシーンへ
            {
                Debug.Log("OP end");
            } else
            {
                StartCoroutine("AddText");
            }
        }
    }

    //動画1再生終了時、動画を止めてこれを呼ぶ
    public void StartOPText1()
    {
        textNum = 0;

        StartCoroutine("SetTextBackGround");
    }

    //動画2再生終了時、動画を止めてこれを呼ぶ
    public void StartOPText2()
    {
        textNum = changeMovie2Num;

        StartCoroutine("SetTextBackGround");
    }

    void InitializedOPText()
    {
        TextBackGround.color = Color.clear;
        OPText.text = "";
    }

    IEnumerator SetTextBackGround()
    {
        InitializedOPText();

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
