using System;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Player Components")]
    [SerializeField] private PlayerData playerData;
    [SerializeField] private Rigidbody2D rb;

    [Header("Reflect Ability Settings")]
    [SerializeField] private float reflectForce = 20f;
    [SerializeField] private float reflectRadius = 3f;
    [SerializeField] private LayerMask reflectableLayer;

    [Header("Player Bullet")]
    [SerializeField] private GameObject leftFacingBullet;
    [SerializeField] private GameObject rightFacingBullet;
    [SerializeField] private Transform firepoint;
    private float m_timer;
    [SerializeField] private float firingRate = 0.2f;

    private Vector2 m_movementInput;
    private bool m_flipped;
    

    private void Update()
    {
        m_movementInput.x = Input.GetAxisRaw("Horizontal");
        m_movementInput.y = Input.GetAxisRaw("Vertical");
        m_movementInput = m_movementInput.normalized;

        if(m_movementInput.x > 0 && m_flipped)
        {
            FlipPlayer();
        }
        else if(m_movementInput.x < 0 && !m_flipped)
        {
            FlipPlayer();
        }

        if (Input.GetKeyDown(KeyCode.X))
        {
            ReflectBullets();
        }

        m_timer += Time.deltaTime;
        if (Input.GetKey(KeyCode.Z))
        {
            if (m_timer >= firingRate)
            {
                Shoot();
                m_timer = 0;
            }
        }
    }

    private void FixedUpdate()
    {
        rb.velocity = new Vector2(m_movementInput.x * playerData.playerDataClass.playerSpeed, m_movementInput.y * playerData.playerDataClass.playerSpeed);
    }

    private void FlipPlayer()
    {
        m_flipped = !m_flipped;

        Vector3 localScale = transform.localScale;
        localScale.x *= -1;
        transform.localScale = localScale;
    }

    private void Shoot()
    {
        if (!m_flipped)
        {
            GameObject bullet = Instantiate(rightFacingBullet, firepoint.position, Quaternion.identity);
        }
        else if(m_flipped)
        {
            GameObject bullet = Instantiate(leftFacingBullet, firepoint.position, Quaternion.identity);
        }
    }

    private void ReflectBullets()
    {
        Collider2D[] bulletsInRange = Physics2D.OverlapCircleAll(transform.position, reflectRadius, reflectableLayer);

        foreach (Collider2D hit in bulletsInRange)
        {
            Bullet bullet = hit.GetComponent<Bullet>();
            Rigidbody2D rb = hit.GetComponent<Rigidbody2D>();

            if (rb != null)
            {
                rb.velocity = Vector2.zero;

                Vector2 direction = (rb.position - (Vector2)transform.position).normalized;

                rb.AddForce(direction * reflectForce, ForceMode2D.Impulse);
            }

            if(bullet != null)
            {
                bullet.reflected = true;
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position, reflectRadius);
    }
}
