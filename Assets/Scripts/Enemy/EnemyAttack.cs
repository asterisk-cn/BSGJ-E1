using Players;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EnemyAttack : MonoBehaviour
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

    //追跡するオブジェクト
    [SerializeField] PlayerCharacter _target;

    //攻撃範囲ようの距離
    [SerializeField] float range = 20;

    //攻撃の確率 高いほど発生しにくい  
    [SerializeField]
    [Header("確率")]
    int attackRate = 90;

    //攻撃の抽選に外れた回数
    [SerializeField] int attackMissCount = 0;

    //ミスの上限値
    [SerializeField] int missLimit = 150;

    //攻撃関数用のフラグ
    public bool isAttack = false;

    public bool isUp = false;
    [HideInInspector]
    public bool isActive ;

    [Header("移動の自動化フラグ")]
    public bool autoMove = true;

    [SerializeField]
    [Header("障害物になるフラグ")]
    private bool stop;

    [SerializeField]
    [Header("留まる秒数")]
    private float waitSecond = 3.0f;

    

    // Start is called before the first frame update
    void Start()
    {
        //エラー防止用
        if (_target == null) { autoMove = false; }
        isActive = true;
    }

    // Update is called once per frame
    void Update()
    {

    }
    /**
     * @brief 敵の移動
     * @author 鈴木宏明
     * @date  '24/4/28
     * @History 手動操作のキー入力を変更
     */
    void Move()
    {   
        //プレイヤーを追跡する
        if (autoMove) 
        {
            Vector3 distance = new Vector3(_target.transform.position.x - this.transform.position.x, 0, _target.transform.position.z - this.transform.position.z);
            //攻撃する範囲かの判定
            if (distance.magnitude <=range) 
            {
                //上限を超えたとき攻撃
                if(attackMissCount >missLimit) { isAttack = true; attackMissCount = 0; };
                //ランダムに攻撃
                //確率で攻撃
                int buff =(int)Random.Range(0,100.0f);
                if(buff < attackRate) { isAttack = true; attackMissCount = 0; }
                else {attackMissCount++;}
            }
            //抽選回数のリセット
            else 
            {
                attackMissCount = 0;
            }
            
            if(!isAttack)
            {
                //攻撃しない場合、移動する
                if (distance.magnitude > moveSpeed) 
                {
                    distance = distance.normalized;
                    //
                    distance.Scale(new Vector3(moveSpeed,0,moveSpeed));
                    //
                    this.transform.position += distance;
                
                }

                
            
            }
            



        }
        //以前のバージョン
        else 
        {
            //前
            if(Input.GetKey(KeyCode.I)) 
            {
                transform.position += moveSpeed * transform.forward;
            }

            //後
            if(Input.GetKey(KeyCode.K)) 
            {
                transform.position -= moveSpeed * transform.forward;
            }

            //右
            if (Input.GetKey(KeyCode.L)) 
            {
                transform.position += moveSpeed * transform.right;
            }

            //左
            if (Input.GetKey(KeyCode.J)) 
            {
                transform.position -= moveSpeed * transform.right;
            }

        }

    }

    void Attack()
    {
        float halfHeight =(defaultHeight-stageHeight)/2;
        if(!isAttack) { return; }

        //武器を降ろす
        if(!isUp) 
        {
            transform.position -= attackSpeed * transform.up;
            //半分振り下ろすとさらに加速
            if (transform.position.y < halfHeight) { transform.position -= attackSpeed * transform.up; }
        }

        //攻撃がステージに到達
        if(!isUp &&transform.position.y <= stageHeight) 
        {
            if (!stop) { isUp = true; }
            else
            {
                isAttack = false;
                StartCoroutine("Stop");
            }
            //ステージに埋まらないようにする
            float buff = transform.localScale.y / 2;
            transform.position = new Vector3(transform.position.x, stageHeight+buff, transform.position.z);
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

    IEnumerator Stop()
    {
        Rigidbody rb = this.gameObject.AddComponent<Rigidbody>();
        rb.useGravity = true;
        rb.constraints = RigidbodyConstraints.FreezePosition;

        BoxCollider collider = this.GetComponent<BoxCollider>();
        //攻撃を無効
        collider.isTrigger = false;

        isActive = false;

        yield return new WaitForSeconds(waitSecond);
        Destroy(this.gameObject);
    }

    void OnTriggerEnter(Collider other)
    {

    }

    private void FixedUpdate()
    {
        if(!isActive) { return; }
        else
        {
            if (!isAttack) { Move(); }

            if(isAttack) { Attack(); }

            //自動操縦の間は攻撃しない
            if (!autoMove)
            {
                //攻撃の実行
                if (Input.GetKey(KeyCode.Space)&&!isAttack) 
                {
                    isAttack = true;
                }
            }
        }
    }
}
