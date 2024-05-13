using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCore : MonoBehaviour
{
    public bool isAlive;
    public int health;

    private int _currentHealth;
    [SerializeField] private List<EnemyAttack> _attackPrefabs;

    void GenerateAttack()
    {

    }

    public void TakeDamage(int damage)
    {

    }

    public void Die()
    {

    }
}
