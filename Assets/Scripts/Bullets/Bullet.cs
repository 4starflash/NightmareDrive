using UnityEngine;
using System;

public class Bullet : MonoBehaviour
{
    [SerializeField] private BulletData m_bulletData;
    [SerializeField] private Rigidbody2D m_rb;

    [Header("Reflect Settings")]
    public bool reflected = false;
    [SerializeField] private float m_rotationSpeed = 800f;


    private void FixedUpdate()
    {
        if(!reflected)
        {
            m_rb.AddForce(transform.up * m_bulletData.bulletDataClass.bulletSpeed);
        }

        if (reflected)
        {
            transform.Rotate(Vector3.forward * m_rotationSpeed * Time.deltaTime);
        }
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.CompareTag("Border") || collider.gameObject.CompareTag("Player"))
        {
            DestroyBullet();
        }
    }


    private void DestroyBullet()
    {
        Destroy(gameObject);
    }

}
