using UnityEngine;
using System;

public class Bullet : MonoBehaviour
{
    [SerializeField] private BulletData bulletData;
    [SerializeField] private Rigidbody2D rb;

    private void FixedUpdate()
    {
        rb.AddForce(transform.up * bulletData.bulletDataClass.bulletSpeed);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Border"))
        {
            Destroy(gameObject);
        }
    }
}
