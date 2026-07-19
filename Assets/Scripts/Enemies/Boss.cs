using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public enum BossState { Idle, Moving, Attacking, Stunned, Dead }

public abstract class Boss : MonoBehaviour
{
    [Header("Boss Components")]
    [SerializeField] protected BossData m_bossData;
    [SerializeField] protected Animator m_anim;
    [SerializeField] protected Rigidbody2D m_rb;
    [SerializeField] protected BossBulletShooter m_bulletShooter;

    [Header("Boss References")]
    [SerializeField] protected Transform m_target;
    [ReadOnly][SerializeField] protected float m_stateTime;

    protected bool m_flipped;

    protected virtual void Start()
    {
        m_bossData.currentHealth = m_bossData.maxHealth;
        SetCurrentState(BossState.Idle);
    }

    protected virtual void Update()
    {
        UpdateState();
        UpdateStateTime();
    }

    protected abstract void UpdateState();

    protected void UpdateStateTime()
    {
        m_stateTime += Time.deltaTime;
    }

    protected void SetCurrentState(BossState state)
    {
        if(m_bossData.currentState != state)
        {
            m_stateTime = 0;
        }

        m_bossData.currentState = state;

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

    protected abstract void IdleBehavior();
    protected abstract void MovingBehavior();
    protected abstract void AttackingBehavior();
    protected abstract void StunnedBehavior();
    protected abstract void DeadBehavior();

    private void UpdateHealth(int value)
    {
        if (m_bossData.currentHealth - value < 0)
        {
            m_bossData.currentHealth = 0;
        }
        else
        {
            m_bossData.currentHealth -= value;
        }

        float currentRatio = m_bossData.currentHealth / m_bossData.maxHealth;
        //OnHealthChange?.Invoke(currentRatio);

        if(m_bossData.currentHealth <= 0)
        {
            SetCurrentState(BossState.Dead);
        }
    }

    protected void CheckFlip()
    {
        if (m_target != null)
        {
            if (m_target.position.x >= transform.position.x && !m_flipped) FlipBoss();
            else if (m_target.position.x < transform.position.x && m_flipped) FlipBoss();
        }
    }

    protected void FlipBoss()
    {
        m_flipped = !m_flipped;

        Vector3 localScale = transform.localScale;
        localScale.x *= -1;
        transform.localScale = localScale;
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.CompareTag("PlayerDamageSource"))
        {
            UpdateHealth(10);
        }

        else if (collider.gameObject.CompareTag("PlayerUlt"))
        {
            UpdateHealth(20);
            SetCurrentState(BossState.Stunned);
        }
    }
}
