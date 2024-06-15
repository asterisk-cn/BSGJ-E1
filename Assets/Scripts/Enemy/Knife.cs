using Enemy;
using GameManagers;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Knife : EnemyAttack
{
    //派手に落ちる部分だけ実装が必要？

    public override void PlaySE()
    {
        AudioManager.Instance.PlaySE("Main_Knife_SE");
    }
}
