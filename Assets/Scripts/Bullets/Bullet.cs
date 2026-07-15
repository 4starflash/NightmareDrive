using UnityEngine;
using System;

public class Bullet : MonoBehaviour
{
    [Header("Bullet Components")]
    [SerializeField] private BulletData m_bulletData;
    [SerializeField] private Rigidbody2D m_rb;

    [Header("Bullet Settings")]
    [SerializeField] private bool m_constantSpeed;

    [Header("Reflect Settings")]
    public bool reflected = false;
    [SerializeField] private float m_rotationSpeed = 800f;

    private float m_timer;

    private void Update()
    {
        m_timer += Time.deltaTime;
        if(m_timer >= m_bulletData.bulletDataClass.bulletDuration)
        {
            DestroyBullet();
        }
    }

    private void FixedUpdate()
    {
        if(!reflected)
        {
            if (!m_constantSpeed)
            {
                m_rb.AddForce(transform.up * m_bulletData.bulletDataClass.bulletSpeed);
            }
            else
            {
                m_rb.velocity = transform.up * m_bulletData.bulletDataClass.bulletSpeed;
            }
        }

        if (reflected)
        {
            transform.Rotate(Vector3.forward * m_rotationSpeed * Time.deltaTime);
        }
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.CompareTag("Border") || collider.gameObject.CompareTag("Player") || collider.gameObject.CompareTag("PlayerUlt"))
        {
            DestroyBullet();
        }
    }


    private void DestroyBullet()
    {
        Destroy(gameObject);
    }

}
