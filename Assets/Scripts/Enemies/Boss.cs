using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public enum BossState { Idle, Moving, Attacking, Stunned, Dead }

public abstract class Boss : MonoBehaviour
{
    [Header("Boss Components")]
    [SerializeField] protected BossData m_bossData;
    [SerializeField] protected Animator anim;
    [SerializeField] protected Rigidbody2D rb;

    protected virtual void Update()
    {
        UpdateState();
    }

    protected abstract void UpdateState();

    protected void SetCurrentState(BossState state)
    {
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
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.CompareTag("PlayerDamageSource"))
        {
            UpdateHealth(10);
        }
    }
}
