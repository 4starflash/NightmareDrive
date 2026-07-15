using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "PlayerData", menuName = "Scriptable Objects/PlayerData")]
public class PlayerData : ScriptableObject
{
    [SerializeField] private float m_maxHealth;
    public float maxHealth { get { return m_maxHealth; } }
    [ReadOnly][SerializeField] private float m_currentHealth;
    public float currentHealth { get => m_currentHealth; set => m_currentHealth = value; }

    [SerializeField] private float m_maxEnergy;
    public float maxEnergy { get { return m_maxEnergy; } }
    [ReadOnly][SerializeField] private float m_currentEnergy;
    public float currentEnergy { get => m_currentEnergy; set => m_currentEnergy = value; }

    [SerializeField] private float m_playerSpeed;
    public float playerSpeed { get { return m_playerSpeed; } }

    [ReadOnly][SerializeField] private PlayerState m_currentState;
    public PlayerState currentState { get { return m_currentState; } set { m_currentState = value; } }

}