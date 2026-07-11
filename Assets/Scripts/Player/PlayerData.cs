using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "PlayerData", menuName = "Scriptable Objects/PlayerData")]
public class PlayerData : ScriptableObject
{
    [SerializeField] private int m_maxHealth;
    public int maxHealth { get { return m_maxHealth; } }

    [ReadOnly][SerializeField] private int m_currentHealth;
    public int currentHealth { get => m_currentHealth; set => m_currentHealth = value; }

    [SerializeField] private float m_playerSpeed;
    public float playerSpeed { get { return m_playerSpeed; } }
}