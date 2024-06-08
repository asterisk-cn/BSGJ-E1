using Players;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace Enemy
{
    public class EnemyCore : MonoBehaviour
    {
        public bool isAlive;
        public int health;

        private int _currentHealth;

        private List<EnemyAttack> _attackPrefabs;
        [SerializeField] private EnemyAttack _hand;
        [SerializeField] private EnemyAttack _knife;
        [SerializeField] private EnemyAttack _pot;
        [SerializeField] private bool _setHand = true;
        [SerializeField] private bool _setKnife = true;
        [SerializeField] private bool _setPot = true;

        [SerializeField] public static readonly List<EnemyAttack> _attackView = new List<EnemyAttack>();

        [SerializeField] private PlayerCore _player;

        [SerializeField] private float _attackStartHeight = 10.0f;
        [SerializeField] private float _stageHeight = 0;

        // [SerializeField] private float _stageLimit_x;
        // [SerializeField] private float _stageLimit_z;
        [SerializeField] private Transform _stageLimit_top_left;
        [SerializeField] private Transform _stageLimit_bottom_right;

        private float _stageLimit_left;
        private float _stageLimit_right;
        private float _stageLimit_front;
        private float _stageLimit_back;

        enum _enemyAttack
        {
            EnemyAttack,
            Hand,
            Knife,
            Pot
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
            if (_attackView.Count >= 2) return;
            ResetAttackPrefabs();
            int index = Random.Range(0, _attackPrefabs.Count);
            var generate = Instantiate(_attackPrefabs[index], new Vector3(0, _attackStartHeight, 0), Quaternion.identity, gameObject.transform);
            var comp = generate.GetComponent<EnemyAttack>();
            // ターゲットの選択
            Transform target = null;
            if (_player.partial == null)
            {
                target = _player.character.transform;
            }
            else
            {
                target = Random.value < 0.5 ? _player.character.transform : _player.partial.transform;
            }
            comp.Initialize(_attackStartHeight, _stageHeight, target);

            if (index == (int)_enemyAttack.Hand)
            {
                float buff_x = Random.Range(_stageLimit_left, _stageLimit_right);
                float buff_z = Random.Range(_stageLimit_front, _stageLimit_back);
                generate.transform.position = new Vector3(buff_x, 10, buff_z);
            }
            _attackView.Add(comp);
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
                GenerateAttack();
            }
        }

        public void TakeDamage(int damage)
        {
            _currentHealth -= damage;
            if (_currentHealth <= 0)
            {
                _currentHealth = 0;
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
