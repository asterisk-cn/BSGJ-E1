using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditorInternal.ReorderableList;

public class TextFloat : MonoBehaviour
{
    public GameObject[] SelectObjects;
    public bool startSelect;
    public float AMP, SPD;
    int selectObjNum;
    Vector3[] DefaultPos;
    int i, nowSelectNum;

    public int testSelectNum; //テスト用


    private void Start()
    {
        selectObjNum = SelectObjects.Length;
        DefaultPos = new Vector3[selectObjNum];
        for(i = 0; i < selectObjNum; i++)
        {
            DefaultPos[i] = SelectObjects[i].transform.position;
        }
    }

    void Update()
    {
        //テスト用
        if(testSelectNum != nowSelectNum)
        {
            ChangeSelectObj(testSelectNum);
        }

        if(startSelect)
            SelectObjects[nowSelectNum].transform.position = DefaultPos[nowSelectNum] + (new Vector3(0, AMP * Mathf.Sin(SPD * Time.time), 0));
    }

    public void ChangeSelectObj(int selectNum)
    {
        nowSelectNum = selectNum;
        for (i = 0; i < selectObjNum; i++)
        {
            SelectObjects[i].transform.position = DefaultPos[i];
        }
    }
}
