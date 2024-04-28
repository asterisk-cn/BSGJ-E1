using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    //�ړ����x
    [SerializeField] float moveSpeed =0.01f;

    //�U���̑��x(�ӂ肨�낵)
    [SerializeField] float attackSpeed = 0.1f;

    //�߂鑬�x
    [SerializeField] float upSpeed = 0.2f;

    //���̍���
    [SerializeField] float defaultHeight = 10.0f;

    //�X�e�[�W�̍���
    [SerializeField] float stageHeight = 0;

    //�U���֐��p�̃t���O
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
     * @brief �G�̈ړ�
     * @author ��؍G��
     * @date  '24/4/28
     * 
     */
    void Move()
    {   
        //�O
        if(Input.GetKey(KeyCode.W)) 
        {
            transform.position += moveSpeed * transform.forward;
        }

        //��
        if(Input.GetKey(KeyCode.S)) 
        {
            transform.position -= moveSpeed * transform.forward;
        }

        //�E
        if (Input.GetKey(KeyCode.D)) 
        {
            transform.position += moveSpeed * transform.right;
        }

        //��
        if (Input.GetKey(KeyCode.A)) 
        {
            transform.position -= moveSpeed * transform.right;
        }

    }

    void Attack()
    {
        float halfHeight =(defaultHeight-stageHeight)/2;

        //������~�낷
        if(!isUp) 
        {
            transform.position -= upSpeed * transform.up;
            //�����U�艺�낷�Ƃ���ɉ���
            if (transform.position.y < halfHeight) { transform.position -= upSpeed * transform.up; }
        }

        //�U�����X�e�[�W�ɓ��B
        if(!isUp &&transform.position.y <= stageHeight) 
        {
            isUp = true;
        }

        //���̍����ɖ߂�
        if(isUp) { transform.position += upSpeed * transform.up; }
        //���̍����ɓ��B
        if(isUp&&transform.position.y >= defaultHeight) 
        {

            transform.position = new Vector3(transform.position.x,defaultHeight,transform.position.z) ;
            //�֐��̏I��
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

        //�U���̎��s
        if(Input.GetKey(KeyCode.Space)&&!isAttack) 
        {
            isAttack = true;
        }
    }
}
