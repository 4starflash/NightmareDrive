using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class SignBoss : Boss
{
    [Header("Bullet Types")]
    [SerializeField] private GameObject[] bulletType;

    private int m_randomAttack;

    protected override void Start()
    {
        base.Start();
    }

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
            m_randomAttack = UnityEngine.Random.Range(0, 2);
        }
    }

    protected override void AttackingBehavior()
    {
        m_rb.velocity = Vector2.zero;

        if(m_randomAttack == 0)
        {
            m_bulletShooter.ShootBullets(bulletType[0], 5, 30f, true);
        }
        else if(m_randomAttack == 1)
        {
            m_bulletShooter.ShootBullets(bulletType[1], 11, 360f, true);
        }

        SetCurrentState(BossState.Idle);

    }

    protected override void StunnedBehavior()
    {
        m_rb.velocity = Vector2.zero;

        if (m_stateTime >= 3f)
        {
            SetCurrentState(BossState.Idle);
        }
    }

    protected override void DeadBehavior()
    {

    }
}
