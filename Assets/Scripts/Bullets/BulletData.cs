using UnityEngine;
using System;

[System.Serializable]
public class BulletDataClass
{
    [SerializeField] private Sprite m_bulletSprite;
    public Sprite bulletSprite { get { return m_bulletSprite; } }
    [SerializeField] private float m_bulletSpeed;
    public float bulletSpeed { get { return m_bulletSpeed; } }
}

[CreateAssetMenu(fileName = "BulletData", menuName = "Scriptable Objects/BulletData")]
public class BulletData : ScriptableObject
{
    [SerializeField] private BulletDataClass m_bulletDataClass;
    public BulletDataClass bulletDataClass { get { return m_bulletDataClass; } }
}
