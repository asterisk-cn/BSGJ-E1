using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FightUIManager : MonoBehaviour
{
    public Image SheffFace_Image;
    public Sprite SheffFaceBatsuIcon;
    public GameObject SheffHat_Obj, Arm_Obj;
    Transform SheffHat_Trans, Arm_Trans;
    public float sheffHP, sheffMaxHP;
    float rate;
    public Vector3 DefaultSheffHatSize, DiffFromHatToArm;

    private void Start()
    {
        SheffHat_Trans = SheffHat_Obj.GetComponent<Transform>();
        Arm_Trans = Arm_Obj.GetComponent<Transform>();
        DefaultSheffHatSize = SheffHat_Trans.localScale;

        sheffHP = sheffMaxHP;
        DiffFromHatToArm = Arm_Trans.position - SheffHat_Trans.position;
    }

    private void Update()
    {
        if(sheffHP >= 0)
        {
            rate = sheffHP / sheffMaxHP;
            SheffHat_Trans.localScale = new Vector3(rate * DefaultSheffHatSize.x, DefaultSheffHatSize.y, DefaultSheffHatSize.z);
            Arm_Trans.position = SheffHat_Trans.position + rate * DiffFromHatToArm;
        }
        else
        {
            SheffFace_Image.sprite = SheffFaceBatsuIcon;
            SheffHat_Obj.SetActive(false);
        }

        //テスト用
        //if (Input.GetMouseButtonDown(0))
        //{
        //    sheffHP -= 1;
        //}
        //if (Input.GetMouseButtonDown(1))
        //{
        //    sheffHP += 10;
        //}
    }
}
