using Enemy;
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
        public float unionCount;
        public int partialHitCount;
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
        // [SerializeField] private GameObject _generatePosition;
        [SerializeField] private List<GameObject> _generatePositions = new List<GameObject>();

        [SerializeField]
        [Header("巨大化の倍率")]
        public float sizeUpRate;

        [SerializeField]
        private EnemyCore _enemy;

        private bool _isAttacked = false;

        [SerializeField][Tooltip("増加量")] private float increaseUnionCount;

        [SerializeField][Tooltip("減少量")] private float decreaseUnionCount;



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
            if (_inputs.leftAttack)
            {
                _enemy.TakeDamage(1);
            }
            if (_inputs.rightAttack)
            {
                _enemy.TakeDamage(1);
            }
        }

        public void UnitePartial()
        {
            _currentParameters.unionCount += increaseUnionCount;
            if (_currentParameters.unionCount >= 6)
            {
                SceneFadeManager.instance.FadeOut("Fight");
            }
            GeneratePartial();
        }

        void GeneratePartial()
        {
            // 生成位置を決定
            var generatePositionTransform = _generatePositions[0].transform;
            foreach (var position in _generatePositions)
            {
                var currentDistance = Vector3.Distance(generatePositionTransform.position, character.transform.position);
                var newDistance = Vector3.Distance(position.transform.position, character.transform.position);
                if (currentDistance < newDistance)
                {
                    generatePositionTransform = position.transform;
                }
            }
            partial = Instantiate(_partialPrefabs[_partialIndex], generatePositionTransform.position, Quaternion.identity);
            partial.SetCore(this);

            _partialIndex++;
            if (_partialIndex >= _partialPrefabs.Count)
            {
                _partialIndex = 0;
            }
        }

        //5/25追加 Suzuki H
        //ダメージ処理用の呼び出し関数
        public void TakeDamage(int damage)
        {
            _currentParameters.health -= damage;
            //ゲームオーバー処理？　リザルト処理に遷移
            if (_currentParameters.health <= 0)
            {
                Die();
            }
        }

        public void TakePartialDamage(int damage)
        {
            _currentParameters.partialHitCount++;
            _currentParameters.unionCount -= decreaseUnionCount;
        }

        void Die()
        {
            isAlive = false;
            MainGameManager.instance.isClear = false;
            SceneFadeManager.instance.FadeOut("Result");
        }

        public int GetCurrentHealth()
        {
            return _currentParameters.health;
        }
    }
}
