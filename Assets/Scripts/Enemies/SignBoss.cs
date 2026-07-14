using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class SignBoss : Boss
{
    protected override void UpdateState()
    {
        if(m_bossData.currentState == BossState.Attacking)
        {
            //perform attack
        }
    }

    protected override void IdleBehavior()
    {

    }

    protected override void MovingBehavior()
    {

    }

    protected override void AttackingBehavior()
    {

    }

    protected override void StunnedBehavior()
    {

    }

    protected override void DeadBehavior()
    {

    }
}
