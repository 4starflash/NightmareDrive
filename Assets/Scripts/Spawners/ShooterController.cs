using System;
using UnityEngine;

public class ShooterController : MonoBehaviour
{
    [SerializeField] private BulletShooter bulletShooter;
    [SerializeField] private GameObject bulletPrefab;

    public static event Action<GameObject> OnShootBullet;

    [SerializeField] private float firingRate = 1f;
    private float timer;


    private void Update()
    {
        timer += Time.deltaTime;

        if (timer >= firingRate)
        {
            OnShootBullet?.Invoke(bulletPrefab);
            timer = 0;
        }
    }
}
