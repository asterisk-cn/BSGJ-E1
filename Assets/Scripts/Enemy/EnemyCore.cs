using GameManagers;
using Players;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using Unity.VisualScripting;
using UnityEngine;

namespace Enemy
{
    public class EnemyCore : MonoBehaviour
    {
        [Header("調整用パラメータ")]
        [Header("敵のパラメータ")]
        public int health;

        [Header("攻撃設定")]
        [SerializeField] private bool _setHand = true;
        [SerializeField] private bool _setKnife = true;
        [SerializeField] private bool _setPot = true;

        [SerializeField][Tooltip("同時に生成する攻撃の最大数")] private int _maxAttackCount = 2;
        [SerializeField][Tooltip("攻撃生成の高さ")] private float _attackStartHeight = 10.0f;
        [Header("-----------------------------")]
        [Space(10)]

        public bool isAlive;

        private int _currentHealth;

        private List<EnemyAttack> _attackPrefabs;
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

        float maxUnionCount;

        void LevelAdjustment()
        {
            maxUnionCount = System.Math.Max(maxUnionCount, _player.GetCurrentUnionCount());
            var ratio = maxUnionCount / _player.GetTargetUnionCount();

            switch (ratio)
            {
                case 0:
                    _setHand = false;
                    _setKnife = true;
                    _setPot = false;
                    break;
                case { } n when (ratio < 0.25):
                    _setHand = false;
                    _setKnife = true;
                    _setPot = false;
                    break;
                case { } n when (ratio < 0.5):
                    _setHand = false;
                    _setKnife = true;
                    _setPot = true;
                    break;
                case { } n when (ratio < 0.75):
                    _setHand = true;
                    _setKnife = true;
                    _setPot = true;
                    break;
            }
        }

        void ResetAttackPrefabs()
        {
            _attackPrefabs = new List<EnemyAttack>();
            if (_setHand) _attackPrefabs.Add(_hand);
            if (_setKnife) _attackPrefabs.Add(_knife);
            if (_setPot) _attackPrefabs.Add(_pot);
        }

        void GenerateAttack()
        {
            CheckAttack();
            if (_attackView.Count >= _maxAttackCount) return;
            ResetAttackPrefabs();
            if (_attackPrefabs.Count == 0) return;
            int index = Random.Range(0, _attackPrefabs.Count);
            var generate = Instantiate(_attackPrefabs[index], new Vector3(0, _attackStartHeight, 0), Quaternion.identity, gameObject.transform);
            var comp = generate.GetComponent<EnemyAttack>();
            // ターゲットの選択
            bool canTargetPlayerCharacter = _player.character != null;
            bool canTargetPlayerPartial = (_player.partial != null && _player.partial.isOnFloor);

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
                target = Random.value < 0.5 ? _player.character.transform : _player.partial.transform;
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
                comp.Initialize(_attackStartHeight, _stageHeight, target);

                if (!comp.GetIsChase())
                {
                    float buff_x = Random.Range(_stageLimit_left, _stageLimit_right);
                    float buff_z = Random.Range(_stageLimit_front, _stageLimit_back);
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
            _currentHealth = health;

            if (MainGameManager.instance.gameState == GameManagers.GameState.Main)
            {
                _stageLimit_left = _stageLimit_top_left.position.x > _stageLimit_bottom_right.position.x ? _stageLimit_bottom_right.position.x : _stageLimit_top_left.position.x;
                _stageLimit_right = _stageLimit_top_left.position.x < _stageLimit_bottom_right.position.x ? _stageLimit_bottom_right.position.x : _stageLimit_top_left.position.x;
                _stageLimit_front = _stageLimit_top_left.position.z > _stageLimit_bottom_right.position.z ? _stageLimit_bottom_right.position.z : _stageLimit_top_left.position.z;
                _stageLimit_back = _stageLimit_top_left.position.z < _stageLimit_bottom_right.position.z ? _stageLimit_bottom_right.position.z : _stageLimit_top_left.position.z;
            }
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
            _currentHealth -= damage;
            if (_currentHealth <= 0)
            {
                _currentHealth = 0;
                //!
                AudioManager.Instance.PlaySE("Fight_FinishiBlaw_SE");
                Die();
            }
        }

        public void Die()
        {
            isAlive = false;
            FightManager.ToResult(true);
        }

        public int GetCurrentHealth()
        {
            return _currentHealth;
        }
    }
}
