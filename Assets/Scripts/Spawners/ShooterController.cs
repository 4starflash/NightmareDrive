using System;
using UnityEngine;

public class ShooterController : MonoBehaviour
{
    [SerializeField] private BulletShooter bulletShooter;
    [SerializeField] private GameObject bulletPrefab;

    public static event Action<GameObject> OnShootBullet;

    private void Start()
    {
        OnShootBullet?.Invoke(bulletPrefab);
    }
}
