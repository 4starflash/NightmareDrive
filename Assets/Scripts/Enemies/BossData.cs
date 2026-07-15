using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "BossData", menuName = "Scriptable Objects/BossData")]
public class BossData : ScriptableObject
{
    [SerializeField] private int m_maxHealth;
    public int maxHealth { get { return m_maxHealth; } }
    [ReadOnly][SerializeField] private int m_currentHealth;
    public int currentHealth { get => m_currentHealth; set => m_currentHealth = value; }

    [ReadOnly][SerializeField] private BossState m_currentState;
    public BossState currentState { get { return m_currentState; } set { m_currentState = value; } }
}