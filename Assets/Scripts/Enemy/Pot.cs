using Enemy;
using GameManagers;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pot : EnemyAttack
{
    public override void PlaySE()
    {
        AudioManager.Instance.PlaySE("Main_Nabe_SE");
    }
}
