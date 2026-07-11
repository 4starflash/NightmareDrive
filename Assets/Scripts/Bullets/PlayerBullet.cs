using UnityEngine;

public class PlayerBullet : MonoBehaviour
{
    [SerializeField] private BulletData bulletData;
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private bool flipped;

    private void FixedUpdate()
    {
        if (!flipped)
        {
            rb.velocity = transform.right * bulletData.bulletDataClass.bulletSpeed;
        }
        else
        {
            rb.velocity = -transform.right * bulletData.bulletDataClass.bulletSpeed;
        }
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.CompareTag("Border") || collider.gameObject.CompareTag("Enemy"))
        {
            DestroyBullet();
        }
    }


    private void DestroyBullet()
    {
        Destroy(gameObject);
    }

}
