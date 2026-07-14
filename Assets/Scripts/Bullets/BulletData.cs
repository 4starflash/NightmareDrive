using UnityEngine;
using System;

[System.Serializable]
public class BulletDataClass
{
    [SerializeField] private float m_bulletSpeed;
    public float bulletSpeed { get { return m_bulletSpeed; } }

    [SerializeField] private float m_bulletDuration;
    public float bulletDuration { get { return m_bulletDuration; } }
}

[CreateAssetMenu(fileName = "BulletData", menuName = "Scriptable Objects/BulletData")]
public class BulletData : ScriptableObject
{
    [SerializeField] private BulletDataClass m_bulletDataClass;
    public BulletDataClass bulletDataClass { get { return m_bulletDataClass; } }
}
