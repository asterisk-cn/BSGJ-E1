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

        private int leftTapCount = 0;
        private int rightTapCount = 0;

        [SerializeField]
        private float powerUpTimer = 0;

        private bool _isAttacked = false;

        [SerializeField]
        EnemyCore enemyCore;


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

            if (MainGameManager.instance.gameState == GameState.Fight && !_isAttacked)
            {
                Attack();
            }

            if (powerUpTimer > powerUpTimeLimit && !_isAttacked)
            {
                _isAttacked = true;

                Debug.Log($"サイズ:{_character1.transform.localScale.x}, 押した回数:{leftTapCount}");
                Debug.Log($"サイズ:{_character2.transform.localScale.x}, 押した回数:{rightTapCount}");
            }
        }

        void Move()
        {
            _character1.Move(_inputs.leftMoveStick);
            _character2.Move(_inputs.rightMoveStick);
        }

        void Attack()
        {
            powerUpTimer += Time.deltaTime;

            if (_inputs.leftAttack)
            {
                leftTapCount++;
                float newScale = _character1.transform.localScale.x + sizeUpRate;

                _character1.ScaleAroundFoot(newScale);

                // アニメーションの再生

                enemyCore.health--;

                _inputs.leftAttack = false;

            }

            if (_inputs.rightAttack)
            {
                rightTapCount++;
                float newScale = _character2.transform.localScale.x + sizeUpRate;

                _character2.ScaleAroundFoot(newScale);

                // アニメーションの再生

                enemyCore.health--;

                _inputs.rightAttack = false;
            }
        }
    }
}
