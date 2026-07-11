using UnityEngine;

public class BulletSpawner : MonoBehaviour
{
    enum ShooterType { SingleShot, MultiShot }
    [SerializeField] private ShooterType shooterType;

    [Header("Spawn Settings")]
    [SerializeField] private int numberOfBullets = 8;
    [SerializeField] private float radius = 3f;
    [SerializeField] private bool aimAtPlayer;
    [SerializeField] private float firingRate = .2f;

    [Header("Shooter Attributes")]
    [SerializeField] private Transform firePoint;
    [SerializeField] private float spreadAngle = 30f;
    private float m_startAngle;
    [SerializeField] private Transform target;

    [Header("Bullet")]
    [SerializeField] private GameObject bulletPrefab;

    private float m_timer;

    private void Start()
    {
        m_startAngle = -spreadAngle / 2f;
    }

    private void Update()
    {
        if ((aimAtPlayer))
        {
            AimAtPlayer();
        }

        m_timer += Time.deltaTime;
        if (m_timer >= firingRate)
        {
            if (shooterType == ShooterType.SingleShot)
            {
                ShootSingleShot();
            }
            else if (shooterType == ShooterType.MultiShot)
            {
                ShootSpreadOfBullets();
            }

            m_timer = 0;
        } 
    }

    private void ShootSingleShot()
    {
        if(target != null)
        {
            Vector3 direction = target.position - transform.position;
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - 90f;
            Quaternion bulletRotation = Quaternion.Euler(0, 0, angle);

            Instantiate(bulletPrefab, firePoint.position, bulletRotation);
        }
        else
        {
            Instantiate(bulletPrefab, firePoint.position, Quaternion.identity);
        }
    }

    // Shoots a ring of bullets
    private void SpawnRingOfBullets(GameObject bulletPrefab)
    {
        for (int i = 0; i < numberOfBullets; i++)
        {
            float angle = i * Mathf.PI * 2 / numberOfBullets;
            float x = Mathf.Cos(angle) * radius;
            float y = Mathf.Sin(angle) * radius;

            Vector3 spawnPosition = new Vector3(x, y, 0f) + transform.position;

            Quaternion spawnRotation = Quaternion.identity;

            float angleDegrees = angle * Mathf.Rad2Deg;
            spawnRotation = Quaternion.Euler(0f, 0f, angleDegrees);

            Instantiate(bulletPrefab, spawnPosition, spawnRotation);
        }
    }

    // Shoots a spread of bullets with the specified angle
    private void ShootSpreadOfBullets()
    {
        float angleStep = spreadAngle / (numberOfBullets - 1);

        for (int i = 0; i < numberOfBullets; i++)
        {
            float currentAngle = m_startAngle + (i * angleStep);
            Quaternion rotation = Quaternion.Euler(0, 0, currentAngle);
            GameObject bullet = Instantiate(bulletPrefab, firePoint.position, rotation);
        }
    }

    // Aims shooter towards the player
    private void AimAtPlayer()
    {
        Vector2 directionToTarget = (target.position - transform.position).normalized;
        float angleRad = Mathf.Atan2(-directionToTarget.y, -directionToTarget.x);
        float angleDeg = angleRad * Mathf.Rad2Deg;
        m_startAngle = angleDeg - spreadAngle / 2 + 90f;
    }
}
