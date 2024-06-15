using GameManagers;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hand : Enemy.EnemyAttack
{
    //ゆらゆら移動用の関数が必要？
    public override void PlaySE()
    {
        AudioManager.Instance.PlaySE("Main_Daipan_SE");
    }
}
