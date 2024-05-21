using Players;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EnemyCore : MonoBehaviour
{
    public bool isAlive;
    public int health;

    private int _currentHealth;

    [SerializeField] private List<EnemyAttack> _attackPrefabs;

    [SerializeField] public static readonly List<EnemyAttack> _attackView = new List<EnemyAttack>();

    // EnemyAttackにアタッチするためのpublic
    public  PlayerCharacter character1;
    public　PlayerCharacter character2;

    void GenerateAttack()
    {
        if (_attackView.Count >= 2) return;
        int index = Random.Range(0, _attackPrefabs.Count);
        var generate = Instantiate(_attackPrefabs[index], new Vector3(0, 10, 0), Quaternion.identity, gameObject.transform);
        var comp = generate.GetComponent<EnemyAttack>();
        _attackView.Add(comp);
    }

    private void Start()
    {
        character1 = GameObject.Find("Player1").GetComponent<PlayerCharacter>();
        character2 = GameObject.Find("Player2").GetComponent<PlayerCharacter>();
        InvokeRepeating("GenerateAttack", 1, 2);
    }

    public void TakeDamage(int damage)
    {

    }

    public void Die()
    {

    }
}
