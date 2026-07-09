using UnityEngine;
using System;

public class BulletShooter : MonoBehaviour
{
    [Header("Spawn Settings")]
    [SerializeField] private int numberOfBullets = 8;
    [SerializeField] private float radius = 3f;

    [Header("Rotation Settings")]
    public bool faceOutward = true;

    private void OnEnable()
    {
        ShooterController.OnShootBullet += SpawnRingOfBullets;
    }

    private void SpawnRingOfBullets(GameObject bulletPrefab)
    {
        for (int i = 0; i < numberOfBullets; i++)
        {
            float angle = i * Mathf.PI * 2 / numberOfBullets;
            float x = Mathf.Cos(angle) * radius;
            float y = Mathf.Sin(angle) * radius;

            Vector3 spawnPosition = new Vector3(x, y, 0f) + transform.position;

            Quaternion spawnRotation = Quaternion.identity;
            if (faceOutward)
            {
                float angleDegrees = angle * Mathf.Rad2Deg;
                spawnRotation = Quaternion.Euler(0f, 0f, angleDegrees);
            }

            Instantiate(bulletPrefab, spawnPosition, spawnRotation);
        }
    }
}