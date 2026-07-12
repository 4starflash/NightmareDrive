using UnityEngine;
using System;

public class PlayerHealth : MonoBehaviour
{
    [SerializeField] private PlayerData m_playerData;

    public static Action<float> OnHealthChange;

    private void Start()
    {
        m_playerData.currentHealth = m_playerData.maxHealth;
    }

    private void UpdateHealth(int value)
    {
        if (m_playerData.currentHealth - value < 0)
        {
            m_playerData.currentHealth = 0;
        }
        else
        {
            m_playerData.currentHealth -= value;
        }

        float currentRatio = m_playerData.currentHealth / m_playerData.maxHealth;
        OnHealthChange?.Invoke(currentRatio);
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.CompareTag("DamageSource"))
        {
            UpdateHealth(10);
        }
    }
}
