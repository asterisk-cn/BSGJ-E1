using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    //移動速度
    [SerializeField] float moveSpeed =0.01f;

    //攻撃の速度(ふりおろし)
    [SerializeField] float attackSpeed = 0.1f;

    //戻る速度
    [SerializeField] float upSpeed = 0.2f;

    //元の高さ
    [SerializeField] float defaultHeight = 10.0f;

    //ステージの高さ
    [SerializeField] float stageHeight = 0;

    //攻撃関数用のフラグ
    public bool isAttack = false;

    public bool isUp = false;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
    /**
     * @brief 敵の移動
     * @author 鈴木宏明
     * @date  '24/4/28
     * 
     */
    void Move()
    {   
        //前
        if(Input.GetKey(KeyCode.W)) 
        {
            transform.position += moveSpeed * transform.forward;
        }

        //後
        if(Input.GetKey(KeyCode.S)) 
        {
            transform.position -= moveSpeed * transform.forward;
        }

        //右
        if (Input.GetKey(KeyCode.D)) 
        {
            transform.position += moveSpeed * transform.right;
        }

        //左
        if (Input.GetKey(KeyCode.A)) 
        {
            transform.position -= moveSpeed * transform.right;
        }

    }

    void Attack()
    {
        float halfHeight =(defaultHeight-stageHeight)/2;

        //武器を降ろす
        if(!isUp) 
        {
            transform.position -= upSpeed * transform.up;
            //半分振り下ろすとさらに加速
            if (transform.position.y < halfHeight) { transform.position -= upSpeed * transform.up; }
        }

        //攻撃がステージに到達
        if(!isUp &&transform.position.y <= stageHeight) 
        {
            isUp = true;
        }

        //元の高さに戻る
        if(isUp) { transform.position += upSpeed * transform.up; }
        //元の高さに到達
        if(isUp&&transform.position.y >= defaultHeight) 
        {

            transform.position = new Vector3(transform.position.x,defaultHeight,transform.position.z) ;
            //関数の終了
            isUp = false;
            isAttack = false;
        }
    }

    void OnTriggerEnter(Collider other)
    {

    }

    private void FixedUpdate()
    {
        Move();

        if(isAttack) { Attack(); }

        //攻撃の実行
        if(Input.GetKey(KeyCode.Space)&&!isAttack) 
        {
            isAttack = true;
        }
    }
}
