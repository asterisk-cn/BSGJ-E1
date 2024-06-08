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
        [Tooltip("体力")] public int health;
        [HideInInspector] public float unionCount;
        [HideInInspector] public int partialHitCount;
    }

    public class PlayerCore : MonoBehaviour
    {

        [Header("調整用パラメータ")]

        [Header("プレイヤーパラメータ")]
        [SerializeField] private PlayerParameters _defaultParameters;

        [Header("合体パラメータ")]
        [SerializeField][Tooltip("増加量")] private float increaseUnionCount;
        [SerializeField][Tooltip("減少量")] private float decreaseUnionCount;
        [SerializeField][Tooltip("目標値")] private float _targetUnionCount = 10;

        [Header("-----------------------------")]
        [Space(10)]

        public bool isAlive;
        PlayerInputs _inputs;
        private PlayerParameters _currentParameters;

        public PlayerCharacter character;
        [HideInInspector] public PlayerPartial partial;

        [SerializeField] List<PlayerPartial> _partialPrefabs = new List<PlayerPartial>();
        private int _partialIndex = 0;

        [SerializeField] private List<GeneratePosition> _generatePositions;

        [SerializeField]
        private EnemyCore _enemy;

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
            if (MainGameManager.instance.gameState == GameState.Main && !_isAttacked)
            {
                Attack();
                if (partial == null)
                {
                    GeneratePartial();
                }
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
                _enemy.TakeDamage((int)_inputs.leftAttackValue);
            }
            if (_inputs.rightAttack)
            {
                _enemy.TakeDamage((int)_inputs.rightAttackValue);
            }
        }

        public void UnitePartial()
        {
            _currentParameters.unionCount += increaseUnionCount;
            if (_currentParameters.unionCount >= _targetUnionCount)
            {
                _currentParameters.unionCount = _targetUnionCount;
                SceneFadeManager.instance.FadeOut("Fight");
            }
            DestroyPartial();
            GeneratePartial();
        }

        void GeneratePartial()
        {
            if (partial != null)
            {
                return;
            }
            // 生成位置を決定
            Transform generatePositionTransform = null;
            foreach (var position in _generatePositions)
            {
                if (position.isCollide)
                {
                    continue;
                }

                if (generatePositionTransform == null)
                {
                    generatePositionTransform = position.transform;
                    continue;
                }

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
            if (_currentParameters.unionCount < 0)
            {
                _currentParameters.unionCount = 0;
            }

            DestroyPartial();
            GeneratePartial();
        }

        void DestroyPartial()
        {
            if (partial != null)
            {
                Destroy(partial.gameObject);
            }
            partial = null;
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

        public float GetCurrentUnionCount()
        {
            return _currentParameters.unionCount;
        }

        public int GetCurrentPartialHitCount()
        {
            return _currentParameters.partialHitCount;
        }

        public float GetTargetUnionCount()
        {
            return _targetUnionCount;
        }
    }
}
