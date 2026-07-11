using UnityEngine;

public class PlayerBullet : MonoBehaviour
{
    [SerializeField] private BulletData m_bulletData;
    [SerializeField] private Rigidbody2D m_rb;
    [SerializeField] private bool m_flipped;

    private void FixedUpdate()
    {
        if (!m_flipped)
        {
            m_rb.velocity = transform.right * m_bulletData.bulletDataClass.bulletSpeed;
        }
        else
        {
            m_rb.velocity = -transform.right * m_bulletData.bulletDataClass.bulletSpeed;
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
