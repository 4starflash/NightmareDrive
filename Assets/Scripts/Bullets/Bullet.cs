using UnityEngine;
using System;

public class Bullet : MonoBehaviour
{
    [SerializeField] private BulletData bulletData;
    [SerializeField] private Rigidbody2D rb;

    [Header("Reflect Settings")]
    private float bulletRadius;
    [SerializeField] private LayerMask reflectableLayer;
    public bool reflected = false;


    private void FixedUpdate()
    {
        if(!reflected)
        {
            rb.AddForce(transform.up * bulletData.bulletDataClass.bulletSpeed);
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
