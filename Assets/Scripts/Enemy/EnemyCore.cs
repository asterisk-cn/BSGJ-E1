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

        [SerializeField] private List<EnemyAttack> _attackPrefabs;

        [SerializeField] public static readonly List<EnemyAttack> _attackView = new List<EnemyAttack>();

        [SerializeField] private PlayerCore _player;

        [SerializeField] private float _attackStartHeight = 10.0f;
        [SerializeField] private float _stageHeight = 0;

        void GenerateAttack()
        {
            CheckAttack();
            if (_attackView.Count >= 2) return;
            int index = Random.Range(0, _attackPrefabs.Count);
            var generate = Instantiate(_attackPrefabs[index], new Vector3(0, 10, 0), Quaternion.identity, gameObject.transform);
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
            _attackView.Add(comp);
        }

        void CheckAttack()
        {
            _attackView.RemoveAll(x => x == null || !x.isActive);
        }

        void Start()
        {
            _currentHealth = health;
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
                Die();
            }
        }

        public void Die()
        {
            isAlive = false;
            FightManager.ToResult(true);
        }
    }
}
