using GameManagers;
using Players;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using Unity.VisualScripting;
using UnityEngine;

namespace Enemy
{
    public class EnemyCore : MonoBehaviour
    {
        [Serializable]
        public class StageLevel
        {
            [Tooltip("この合体数まではこのレベルが適用される")] public int unionCount;
            [Tooltip("ナイフの攻撃の確率")] public float knifeRate;
            [Tooltip("鍋の攻撃の確率")] public float potRate;
            [Tooltip("手の攻撃の確率")] public float handRate;
        }

        [Header("調整用パラメータ")]
        [Header("敵のパラメータ")]
        public int health;

        [SerializeField][Tooltip("同時に生成する攻撃の最大数")] private int _maxAttackCount = 2;
        [SerializeField][Tooltip("攻撃生成の高さ")] private float _attackStartHeight = 10.0f;

        [Header("レベル調整")]
        [SerializeField][Tooltip("ステージレベル")] private List<StageLevel> _stageLevels;

        [Header("カメラの揺れ")]
        [SerializeField][Tooltip("カメラの揺れの時間")] private float _shakeDuration = 0.5f;
        [SerializeField][Tooltip("カメラの揺れの強さ（位置）")] private float _shakePosMagnitude = 0.1f;
        [SerializeField][Tooltip("カメラの揺れの強さ（回転）")] private float _shakeRotMagnitude = 0.1f;
        [Header("-----------------------------")]
        [Space(10)]

        public bool isAlive;

        private int _currentHealth;

        // private List<EnemyAttack> _attackPrefabs;
        [SerializeField] private EnemyAttack _hand;
        [SerializeField] private EnemyAttack _knife;
        [SerializeField] private EnemyAttack _pot;

        [SerializeField] public static readonly List<EnemyAttack> _attackView = new List<EnemyAttack>();

        [SerializeField] private PlayerCore _player;

        [SerializeField] private float _stageHeight = 0;

        // [SerializeField] private float _stageLimit_x;
        // [SerializeField] private float _stageLimit_z;
        [SerializeField] private Transform _stageLimit_top_left;
        [SerializeField] private Transform _stageLimit_bottom_right;

        private float _stageLimit_left;
        private float _stageLimit_right;
        private float _stageLimit_front;
        private float _stageLimit_back;

        private float _handRate;
        private float _knifeRate;
        private float _potRate;

        float maxUnionCount;

        public Animator _animator;
        private AnimatorStateInfo _animatorStateInfo;
        private AnimatorClipInfo[] _animatorClip;
        private float _stateTime;

        private ShakeCamera _camera;

        void LevelAdjustment()
        {
            maxUnionCount = System.Math.Max(maxUnionCount, _player.GetCurrentUnionCount());
            // var ratio = maxUnionCount / _player.GetTargetUnionCount();

            for (int i = 0; i < _stageLevels.Count; i++)
            {
                if (maxUnionCount < _stageLevels[i].unionCount)
                {
                    _handRate = _stageLevels[i].handRate;
                    _knifeRate = _stageLevels[i].knifeRate;
                    _potRate = _stageLevels[i].potRate;
                    break;
                }
            }
        }

        // void ResetAttackPrefabs()
        // {
        //     _attackPrefabs = new List<EnemyAttack>();
        // if (_setHand) _attackPrefabs.Add(_hand);
        // if (_setKnife) _attackPrefabs.Add(_knife);
        // if (_setPot) _attackPrefabs.Add(_pot);
        // }

        void GenerateAttack()
        {
            CheckAttack();
            if (_attackView.Count >= _maxAttackCount) return;
            // ResetAttackPrefabs();
            // if (_attackPrefabs.Count == 0) return;
            if (_handRate + _knifeRate + _potRate == 0) return;

            // ターゲットの選択
            bool canTargetPlayerCharacter = _player.character != null;
            bool canTargetPlayerPartial = _player.partial != null && _player.partial.isOnFloor;

            foreach (var obj in _attackView)
            {
                if (obj._targetTransform == _player.character.transform)
                {
                    canTargetPlayerCharacter = false;
                }
                if (obj._targetTransform == _player.partial.transform)
                {
                    canTargetPlayerPartial = false;
                }
            }

            Transform target = null;

            if (canTargetPlayerCharacter && canTargetPlayerPartial)
            {
                target = UnityEngine.Random.value < 0.5 ? _player.character.transform : _player.partial.transform;
            }
            else if (canTargetPlayerCharacter)
            {
                target = _player.character.transform;
            }
            else if (canTargetPlayerPartial)
            {
                target = _player.partial.transform;
            }

            if (target != null)
            {
                GameObject generate = null;
                float rate = UnityEngine.Random.value * (_handRate + _knifeRate + _potRate);
                if (rate < _handRate)
                {
                    generate = Instantiate(_hand.gameObject, new Vector3(0, _attackStartHeight, 0), Quaternion.identity, gameObject.transform);
                }
                else if (rate < _handRate + _knifeRate)
                {
                    generate = Instantiate(_knife.gameObject, new Vector3(0, _attackStartHeight, 0), Quaternion.identity, gameObject.transform);
                }
                else
                {
                    generate = Instantiate(_pot.gameObject, new Vector3(0, _attackStartHeight, 0), Quaternion.identity, gameObject.transform);
                }

                // int index = UnityEngine.Random.Range(0, _attackPrefabs.Count);
                // var generate = Instantiate(_attackPrefabs[index], new Vector3(0, _attackStartHeight, 0), Quaternion.identity, gameObject.transform);
                var comp = generate.GetComponent<EnemyAttack>();
                comp.SetCore(this);
                comp.Initialize(_attackStartHeight, _stageHeight, target);

                if (!comp.GetIsChase())
                {
                    float buff_x = UnityEngine.Random.Range(_stageLimit_left, _stageLimit_right);
                    float buff_z = UnityEngine.Random.Range(_stageLimit_front, _stageLimit_back);
                    generate.transform.position = new Vector3(buff_x, _attackStartHeight, buff_z);
                }

                _attackView.Add(comp);
            }
        }

        void CheckAttack()
        {
            _attackView.RemoveAll(x => x == null || !x.isActive);
        }

        void Start()
        {

            if(MainGameManager.instance.gameState == GameState.Fight)
            {
                _animator = GetComponentInChildren<Animator>();
            }
            _currentHealth = health;

            if (MainGameManager.instance.gameState == GameManagers.GameState.Main)
            {
                _stageLimit_left = _stageLimit_top_left.position.x > _stageLimit_bottom_right.position.x ? _stageLimit_bottom_right.position.x : _stageLimit_top_left.position.x;
                _stageLimit_right = _stageLimit_top_left.position.x < _stageLimit_bottom_right.position.x ? _stageLimit_bottom_right.position.x : _stageLimit_top_left.position.x;
                _stageLimit_front = _stageLimit_top_left.position.z > _stageLimit_bottom_right.position.z ? _stageLimit_bottom_right.position.z : _stageLimit_top_left.position.z;
                _stageLimit_back = _stageLimit_top_left.position.z < _stageLimit_bottom_right.position.z ? _stageLimit_bottom_right.position.z : _stageLimit_top_left.position.z;

                _camera = Camera.main.GetComponent<ShakeCamera>();
            }
            isAlive = true;
        }

        void Update()
        {
            if (MainGameManager.instance.gameState == GameManagers.GameState.Main)
            {
                LevelAdjustment();
                GenerateAttack();
            }
        }

        public void TakeDamage(int damage)
        {
            _animator.SetTrigger("Damage");
            _currentHealth -= damage;
            if (_currentHealth <= 0)
            {
                _animator.SetTrigger("Down");
                _currentHealth = 0;
                //!
                if (isAlive)AudioManager.Instance.PlaySE("Fight_FinishiBlaw_SE");
                // Die();
            }
        }

        public void Die()
        {
            isAlive = false;
            GameTimeManager.instance.StopTimer();
            FightManager.ToResult(true);
        }

        public int GetCurrentHealth()
        {
            return _currentHealth;
        }

        public void ShakeCamera()
        {
            _camera.Shake(_shakeDuration, _shakePosMagnitude, _shakeRotMagnitude);
        }
    }
}
