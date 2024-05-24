using GameManagers;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace Players
{
    [System.Serializable]
    class PlayerParameters
    {
        public int health;
        public int unionCount;
    }

    public class PlayerCore : MonoBehaviour
    {
        public bool isAlive;
        PlayerInputs _inputs;

        [SerializeField] private PlayerParameters _defaultParameters;
        private PlayerParameters _currentParameters;

        public PlayerCharacter character;
        [HideInInspector] public PlayerPartial partial;

        [SerializeField] List<PlayerPartial> _partialPrefabs = new List<PlayerPartial>();
        private int _partialIndex = 0;
        [SerializeField] private GameObject _generatePosition;

        [SerializeField]
        [Header("パワーアップの時間制限")]
        public float powerUpTimeLimit;

        [SerializeField]
        [Header("巨大化の倍率")]
        public float sizeUpRate;

        private int tapCount = 0;

        [SerializeField]
        private float powerUpTimer = 0;

        private bool _isAttacked = false;

        void Awake()
        {
            _inputs = GetComponent<PlayerInputs>();

            _currentParameters = _defaultParameters;
        }

        // Start is called before the first frame update
        void Start()
        {
            if (MainGameManager.instance.gameState == GameState.Main)
            {
                GeneratePartial();
            }
        }

        // Update is called once per frame
        void Update()
        {
            if (MainGameManager.instance.gameState == GameState.Fight && !_isAttacked)
            {
                Attack();
            }
        }

        private void FixedUpdate()
        {
            Move();

            if (powerUpTimer > powerUpTimeLimit && !_isAttacked)
            {
                _isAttacked = true;

                Debug.Log($"サイズ:{character.transform.localScale.x}, 押した回数:{tapCount}");
            }
        }

        void Move()
        {
            character.Move(_inputs.leftMoveStick);
            if (partial != null)
            {
                partial?.Move(_inputs.rightMoveStick);
            }
        }

        void Attack()
        {
            powerUpTimer += Time.deltaTime;

            if (_inputs.leftAttack)
            {
                PowerUp();
            }
            if (_inputs.rightAttack)
            {
                PowerUp();
            }
        }

        void PowerUp()
        {
            tapCount++;
            float newScale = character.transform.localScale.x + sizeUpRate;

            character.ScaleAroundFoot(newScale);
        }

        public void UnitePartial()
        {
            _currentParameters.unionCount++;
            GeneratePartial();
        }

        void GeneratePartial()
        {
            if (_partialIndex < _partialPrefabs.Count)
            {
                partial = Instantiate(_partialPrefabs[_partialIndex], _generatePosition.transform.position, Quaternion.identity);
                partial.transform.parent = transform;

                _partialIndex++;
            }
            else
            {
                // TODO: RunManagerを呼び出してゲーム終了処理を行う
                MainGameManager.instance.LoadScene("Fight");
            }
        }
    }
}
