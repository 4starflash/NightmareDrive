using UnityEngine;

public class BossBulletShooter : MonoBehaviour
{
    enum ShooterType { SingleShot, MultiShot }
    [SerializeField] private ShooterType m_shooterType;

    enum RotationType { NoSpin, Spin }
    [SerializeField] private RotationType m_rotationType;

    [Header("Spawn Settings")]
    [SerializeField] private int m_numberOfBullets = 8;
    [SerializeField] private float m_radius = 3f;
    [SerializeField] private bool m_aimAtPlayer;
    [SerializeField] private float m_firingRate = .2f;
    [SerializeField] private float m_rotationAngle;

    [Header("Shooter Attributes")]
    [SerializeField] private Transform m_firePoint;
    [SerializeField] private float m_spreadAngle = 30f;
    private float m_startAngle;
    [SerializeField] private Transform m_target;

    [Header("Bullet")]
    [SerializeField] private GameObject m_bulletPrefab;


    private void Start()
    {
        m_startAngle = -m_spreadAngle / 2f;
    }


    private void Update()
    {

        /*
        if (m_rotationType == RotationType.Spin)
        {
            m_startAngle = m_startAngle + m_rotationAngle;

            if (m_startAngle >= 360f)
            {
                m_startAngle = m_startAngle - 360f;
            }
        }
        */
    }

    public void ShootBullets(GameObject bullet, int numberOfBullets, float spreadAngle, bool aim)
    {
        m_bulletPrefab = bullet;
        m_numberOfBullets = numberOfBullets;
        m_spreadAngle = spreadAngle;

        if (aim)
        {
            AimAtPlayer();
        }
        else if (!aim)
        {
            //set start rotation
        }

            ChooseShootType();
    }

    private void ChooseShootType()
    {
        if (m_shooterType == ShooterType.SingleShot)
        {
            ShootSingleShot();
        }
        else if (m_shooterType == ShooterType.MultiShot)
        {
            ShootSpreadOfBullets();
        }
    }

    private void ShootSingleShot()
    {
        if(m_target != null)
        {
            Vector3 direction = m_target.position - transform.position;
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - 90f;
            Quaternion bulletRotation = Quaternion.Euler(0, 0, angle);

            Instantiate(m_bulletPrefab, m_firePoint.position, bulletRotation);
        }
        else
        {
            Instantiate(m_bulletPrefab, m_firePoint.position, Quaternion.identity);
        }
    }

    // Shoots a ring of bullets
    private void SpawnRingOfBullets(GameObject bulletPrefab)
    {
        for (int i = 0; i < m_numberOfBullets; i++)
        {
            float angle = i * Mathf.PI * 2 / m_numberOfBullets;
            float x = Mathf.Cos(angle) * m_radius;
            float y = Mathf.Sin(angle) * m_radius;

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
        float angleStep = m_spreadAngle / (m_numberOfBullets - 1);

        for (int i = 0; i < m_numberOfBullets; i++)
        {
            float currentAngle = m_startAngle + (i * angleStep);
            Quaternion rotation = Quaternion.Euler(0, 0, currentAngle);
            GameObject bullet = Instantiate(m_bulletPrefab, m_firePoint.position, rotation);
        }
    }

    // Aims shooter towards the player
    private void AimAtPlayer()
    {
        Vector2 directionToTarget = (m_target.position - transform.position).normalized;
        float angleRad = Mathf.Atan2(-directionToTarget.y, -directionToTarget.x);
        float angleDeg = angleRad * Mathf.Rad2Deg;
        m_startAngle = angleDeg - m_spreadAngle / 2 + 90f;
    }
}
