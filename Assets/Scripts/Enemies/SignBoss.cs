using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class SignBoss : Boss
{
    [Header("Bullet Types")]
    [SerializeField] private GameObject[] bulletType;

    protected override void UpdateState()
    {
        switch (m_bossData.currentState)
        {
            case BossState.Idle:
                IdleBehavior();
                break;
            case BossState.Moving:
                MovingBehavior();
                break;
            case BossState.Attacking:
                AttackingBehavior();
                break;
            case BossState.Stunned:
                StunnedBehavior();
                break;
            case BossState.Dead:
                DeadBehavior();
                break;
        }
    }

    protected override void IdleBehavior()
    {
        if (m_stateTime >= 3f)
        {
            SetCurrentState(BossState.Moving);
        }
    }

    protected override void MovingBehavior()
    {
        if(m_stateTime < 2f)
        {
            if (m_target != null)
            {
                Vector2 direction = (m_target.position - transform.position).normalized;
                m_rb.AddForce(direction * 1f);
            }
        }

        else
        {
            SetCurrentState(BossState.Attacking);
        }
    }

    protected override void AttackingBehavior()
    {
        m_rb.velocity = Vector2.zero;
        m_bulletShooter.ShootBullets(bulletType[0], 5, 30f, true);
        SetCurrentState(BossState.Idle);
    }

    protected override void StunnedBehavior()
    {

    }

    protected override void DeadBehavior()
    {

    }
}
