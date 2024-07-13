using Enemy;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationEvent : MonoBehaviour
{
    public EnemyCore _enemyCore;

    private void Start()
    {
        _enemyCore = GetComponentInParent<EnemyCore>();
    }
    public void AnimEve()
    {
        _enemyCore.AfterDie();
    }
}
