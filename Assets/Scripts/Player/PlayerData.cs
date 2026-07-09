using UnityEngine;
using System;

[System.Serializable]
public class PlayerDataClass
{
    [SerializeField] private float m_playerHp;
    public float playerHp { get { return m_playerHp; } }
    [SerializeField] private float m_playerSpeed;
    public float playerSpeed { get { return m_playerSpeed; } }
}

[CreateAssetMenu(fileName = "PlayerData", menuName = "Scriptable Objects/PlayerData")]
public class PlayerData : ScriptableObject
{
    [SerializeField] private PlayerDataClass m_playerDataClass;
    public PlayerDataClass playerDataClass { get { return m_playerDataClass; } }
}