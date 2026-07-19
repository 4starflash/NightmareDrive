using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class EyeBoss : Boss
{
    [Header("Bullet Types")]
    [SerializeField] private GameObject[] bulletType;

    private bool m_attacking;

    private int m_randomAttack;

    private bool m_transitionIn;
    private bool m_transitionOut;

    [Header("Movement Settings")]
    [SerializeField] private float m_glideDuration;
    [SerializeField] private float m_glideForce;

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
                if (!m_attacking)
                {
                    AttackingBehavior();
                    m_attacking = true;
                }
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
        CheckFlip();

        if (m_stateTime >= 3f)
        {
            SetCurrentState(BossState.Moving);
        }
    }

    protected override void MovingBehavior()
    {
        if (!m_transitionIn)
        {
            m_anim.SetTrigger("startGlide");
            CheckFlip();
            m_transitionIn = true;
        }

        else if (m_transitionIn && m_stateTime < m_glideDuration)
        {
            if (m_target != null)
            {
                Vector2 direction = (m_target.position - transform.position).normalized;
                m_rb.AddForce(direction * m_glideForce);
            }

            if(m_rb.velocity.x < 0 && m_flipped) FlipBoss();
            else if(m_rb.velocity.x > 0 && !m_flipped) FlipBoss();

        }

        else if (!m_transitionOut && (m_stateTime >= m_glideDuration))
        {
            m_anim.SetBool("glide", false);
            m_anim.SetTrigger("endGlide");
            m_transitionOut = true;
        }
    }

    protected override void AttackingBehavior()
    {
        m_anim.SetTrigger("shootForward");
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

    private void TransitionToGlide()
    {
        m_anim.SetBool("glide", true);
        m_anim.ResetTrigger("startGlide");
    }

    private void TransitionToAttack()
    {
        m_anim.ResetTrigger("endGlide");
        m_transitionOut = false;
        m_transitionIn = false;

        CheckFlip();
        SetCurrentState(BossState.Attacking);
        m_randomAttack = UnityEngine.Random.Range(0, 2);
    }

    private void ShootBullets()
    {
        if (m_randomAttack == 0)
        {
            m_bulletShooter.ShootBullets(BossBulletShooter.ShooterType.MultiShot, bulletType[0], 5, 60f, true);
        }
        else if (m_randomAttack == 1)
        {
            m_bulletShooter.ShootBullets(BossBulletShooter.ShooterType.SingleShot, bulletType[1], 1, 30f, true);
        }
    }

    private void EndAttack()
    {
        SetCurrentState(BossState.Idle);
        m_anim.ResetTrigger("shootForward");
        m_attacking = false;
    }

    private void StopMovement()
    {
        m_rb.velocity = Vector2.zero;
    }
}
