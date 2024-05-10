using GameManagers;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace Players
{
    public class PlayerCore : MonoBehaviour
    {
        public bool isAlive;
        PlayerInputs _inputs;

        [SerializeField] private PlayerCharacter _character1;
        [SerializeField] private PlayerCharacter _character2;

        [SerializeField]
        [Header("パワーアップの時間制限")]
        public float powerUpTimeLimit;

        [SerializeField]
        [Header("巨大化の倍率")]
        public float sizeUpRate;

        private enum InputPhase 
        {
            None,
            WaitForNextPress,
            WaitForNextRelease
        }

        private InputPhase _inputPhase=InputPhase.None;

        private int tapCount = 0;

        [SerializeField]
        private float powerUpTimer = 0;


        // Start is called before the first frame update
        void Start()
        {
            _inputs = GetComponentInParent<PlayerInputs>();
        }

        // Update is called once per frame
        void Update()
        {

        }

        private void FixedUpdate()
        {
            Move();
        }

        void Move()
        {
            _character1.Move(_inputs.leftMoveStick);
            _character2.Move(_inputs.rightMoveStick);
        }

        void Attack()
        {
            //�V�[������
            if (MainGameManager.instance.gameState != GameState.Fight) 
            {
                return;
            }
            else 
            {
                

                if (powerUpTimer > powerUpTimeLimit) 
                {
                    Debug.Log($"�X�P�[��:{transform.localScale.x}");
                    Debug.Log($"�A�Ő�:{tapCount}");
                }
                else 
                {
                    powerUpTimer += Time.deltaTime;

                    if (_inputPhase == InputPhase.None &&
                        Input.GetMouseButtonDown((int)MouseButton.Left)
                        /*Input.GetKeyDown(KeyCode.Space)*/) 
                    {
                        tapCount++;
                        Vector3 ScaleChange = new Vector3(sizeUpRate,sizeUpRate,sizeUpRate);
                        this.transform.localScale += ScaleChange ;
                        _inputPhase = InputPhase.WaitForNextRelease;
                        Debug.Log("�L�[���͇@");
                    }

                    if(_inputPhase ==InputPhase.WaitForNextRelease&&
                        Input.GetMouseButtonDown((int)MouseButton.Left)==false
                        //Input.GetKey(KeyCode.Space)==false 
                        /*Input.GetKeyUp(KeyCode.Space)*/) 
                    {
                        _inputPhase =InputPhase.WaitForNextPress;
                        Debug.Log("�L�[�����ꂽ");
                    }

                    if (_inputPhase == InputPhase.WaitForNextPress &&
                        Input.GetMouseButtonDown((int)MouseButton.Left)
                        /*Input.GetKeyDown(KeyCode.Space)*/)
                    {
                        tapCount++;
                        Vector3 ScaleChange = new Vector3(sizeUpRate, sizeUpRate, sizeUpRate);
                        this.transform.localScale += ScaleChange; 
                        _inputPhase = InputPhase.WaitForNextRelease;
                        Debug.Log("�L�[���͇A");
                    }
                
                }

            }
        }
    }
}
