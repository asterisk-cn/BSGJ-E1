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

        [Header("変換割合")]
        [SerializeField] private int _conversionRate;

        [Header("-----------------------------")]
        [Space(10)]

        public bool isAlive;
        PlayerInputs _inputs;
        private PlayerParameters _currentParameters;

        public PlayerCharacter character;
        [HideInInspector] public PlayerPartial partial;

        [SerializeField] List<PlayerPartial> _partialPrefabs = new List<PlayerPartial>();
        [SerializeField] List<PlayerPartial> _soulPrefabs = new List<PlayerPartial>();
        private int _partialIndex = 0;

        [SerializeField] private List<GeneratePosition> _generatePositions;

        [SerializeField]
        private EnemyCore _enemy;

        private bool _isAttacked = false;

        private Animator _animator;

        void Awake()
        {
            _inputs = GetComponent<PlayerInputs>();

            _currentParameters = _defaultParameters;

            // if(MainGameManager.instance.gameState == GameState.Fight)
            // {
            //     _animator = GetComponentInChildren<Animator>();
            // }
        }

        // Start is called before the first frame update
        void Start()
        {
            if (MainGameManager.instance.gameState == GameState.Main)
            {
                GeneratePartial();
            }

            if (MainGameManager.instance.gameState == GameState.Fight)
            {
                _animator = GetComponentInChildren<Animator>();
            }
        }

        // Update is called once per frame
        void Update()
        {
            if (MainGameManager.instance.gameState == GameState.Main && !_isAttacked)
            {
                if (partial == null)
                {
                    GeneratePartial();
                }
            }
            if (MainGameManager.instance.gameState == GameState.Fight)
            {
                Attack();
            }
        }

        private void FixedUpdate()
        {
            if (SceneFadeManager.instance.isFadeIn || SceneFadeManager.instance.isFadeOut) return;
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
            if (MainGameManager.instance.gameState != GameState.Fight) return;
            if (_enemy.GetCurrentHealth() <= 0) return;
            if (_inputs.leftAttack)
            {
                _animator.SetTrigger("LeftPunch");
                if (_inputs.UseJoycon)
                {
                    _animator.speed = _inputs.leftAttackValue;
                }
                if (_enemy.isAlive)
                {
                    if (_inputs.UseJoycon)
                    {
                        _enemy.TakeDamage((int)_inputs.leftAttackValue / _conversionRate);
                    }
                    else
                    {
                        _enemy.TakeDamage((int)_inputs.leftAttackValue);
                    }
                    AudioManager.Instance.PlaySE("Fight_Punchi&Main_Hit_SE");
                    _inputs.RumbleLeft(160, 320, 0.8f, 0.6f);
                }
            }
            if (_inputs.rightAttack)
            {
                _animator.SetTrigger("RightPunch");
                if (_inputs.UseJoycon)
                {
                    _animator.speed = _inputs.rightAttackValue;
                }
                if (_enemy.isAlive)
                {   
                    if (_inputs.UseJoycon)
                    {
                        _enemy.TakeDamage((int)_inputs.rightAttackValue / _conversionRate);
                    }
                    else
                    {
                        _enemy.TakeDamage((int)_inputs.rightAttackValue);
                    }
                    AudioManager.Instance.PlaySE("Fight_Punchi&Main_Hit_SE");
                    _inputs.RumbleRight(160, 320, 0.8f, 0.6f);
                }
            }
        }

        public void UnitePartial()
        {
            _currentParameters.unionCount += increaseUnionCount;

            AudioManager.Instance.PlaySE("Main_Gattai_SE");
            if (_currentParameters.unionCount >= _targetUnionCount)
            {
                _currentParameters.unionCount = _targetUnionCount;
                MainGameManager.instance.SetScore(GameTimeManager.instance.GetTime(), _currentParameters.partialHitCount);
                SceneFadeManager.instance.FadeOut("MidMovie");
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

            PlayerPartial partialPrefab = null;
            if (_partialIndex < _partialPrefabs.Count)
            {
                partialPrefab = _partialPrefabs[_partialIndex];
                _partialIndex++;
            }
            else
            {
                partialPrefab = _soulPrefabs[Random.Range(0, _soulPrefabs.Count)];
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

            if (generatePositionTransform == null)
            {
                return;
            }

            partial = Instantiate(partialPrefab, generatePositionTransform.position, Quaternion.identity);
            partial.SetCore(this);
        }

        //5/25追加 Suzuki H
        //ダメージ処理用の呼び出し関数
        public void TakeDamage(int damage)
        {
            if (_targetUnionCount <= _currentParameters.unionCount)
            {
                return;
            }

            _currentParameters.health -= damage;

            AudioManager.Instance.PlaySE("Fight_Punchi&Main_Hit_SE");
            _inputs.RumbleLeft(160, 320, 0.8f, 0.6f);
            //ゲームオーバー処理？　リザルト処理に遷移
            if (_currentParameters.health <= 0)
            {
                Die();
            }
        }

        public void TakePartialDamage(int damage)
        {
            if (_targetUnionCount <= _currentParameters.unionCount)
            {
                return;
            }

            _currentParameters.partialHitCount++;
            _currentParameters.unionCount -= decreaseUnionCount;
            if (_currentParameters.unionCount < 0)
            {
                _currentParameters.unionCount = 0;
            }

            AudioManager.Instance.PlaySE("Main_SoulDeth_SE");
            _inputs.RumbleRight(160, 320, 0.6f, 0.4f);

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
            GameTimeManager.instance.StopTimer();
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
