using System;
using UnityEngine;

public enum PlayerState { Idle, Running, Reflecting, Attacking, Dead }

public class PlayerController : MonoBehaviour
{
    [Header("Player Components")]
    [SerializeField] private PlayerData m_playerData;
    [SerializeField] private Rigidbody2D m_rb;
    [SerializeField] private Animator m_anim;

    [Header("Reflect Ability Settings")]
    [SerializeField] private float m_reflectForce = 20f;
    [SerializeField] private float m_reflectRadius = 3f;
    [SerializeField] private LayerMask m_reflectableLayer;

    [Header("Player Bullet")]
    [SerializeField] private GameObject m_leftFacingBullet;
    [SerializeField] private GameObject m_rightFacingBullet;
    [SerializeField] private Transform m_firepoint;
    private float m_timer;
    [SerializeField] private float m_firingRate = 0.2f;

    private Vector2 m_movementInput;
    private bool m_flipped;
    
    private void Start()
    {
        SetCurrentState(PlayerState.Idle);
    }

    private void Update()
    {
        m_movementInput.x = Input.GetAxisRaw("Horizontal");
        m_movementInput.y = Input.GetAxisRaw("Vertical");
        m_movementInput = m_movementInput.normalized;

        if (m_rb.velocity != Vector2.zero && m_playerData.currentState != PlayerState.Attacking)
        {
            SetCurrentState(PlayerState.Running);
        }
        else if(m_rb.velocity == Vector2.zero && m_playerData.currentState != PlayerState.Attacking)
        {
            SetCurrentState(PlayerState.Idle);
        }

        // Flip player when turning
        if (m_movementInput.x > 0 && m_flipped)
        {
            FlipPlayer();
        }
        else if (m_movementInput.x < 0 && !m_flipped)
        {
            FlipPlayer();
        }

        // X to reflect or parry
        if (Input.GetKeyDown(KeyCode.X))
        {
            ReflectBullets();
        }

        // X to shoot bullets
        m_timer += Time.deltaTime;
        if (Input.GetKey(KeyCode.Z))
        {
            if (m_timer >= m_firingRate)
            {
                Shoot();
                m_timer = 0;
            }
        }

        // C to melee
        if (Input.GetKeyDown(KeyCode.C) && m_playerData.currentState != PlayerState.Attacking)
        {
            SetCurrentState(PlayerState.Attacking);
        }
    }

    private void FixedUpdate()
    {
        m_rb.velocity = new Vector2(m_movementInput.x * m_playerData.playerSpeed, m_movementInput.y * m_playerData.playerSpeed);
    }

    private void SetCurrentState(PlayerState state)
    {
        m_playerData.currentState = state;

        switch (m_playerData.currentState)
        {
            case PlayerState.Idle:
                m_anim.SetBool("run", false);
                break;
            case PlayerState.Running:
                m_anim.SetBool("run", true);
                break;
            case PlayerState.Attacking:
                m_anim.SetTrigger("melee");
                break;
        }
    }

    private void ExitActionState()
    {
        if(m_rb.velocity != Vector2.zero)
        {
            m_playerData.currentState = PlayerState.Running;
        }
        else
        {
            m_playerData.currentState = PlayerState.Idle;
        }
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
            GameObject bullet = Instantiate(m_rightFacingBullet, m_firepoint.position, Quaternion.identity);
        }
        else if(m_flipped)
        {
            GameObject bullet = Instantiate(m_leftFacingBullet, m_firepoint.position, Quaternion.identity);
        }
    }

    private void ReflectBullets()
    {
        Collider2D[] bulletsInRange = Physics2D.OverlapCircleAll(transform.position, m_reflectRadius, m_reflectableLayer);

        foreach (Collider2D hit in bulletsInRange)
        {
            Bullet bullet = hit.GetComponent<Bullet>();
            Rigidbody2D rb = hit.GetComponent<Rigidbody2D>();

            if (rb != null)
            {
                rb.velocity = Vector2.zero;

                Vector2 direction = (rb.position - (Vector2)transform.position).normalized;

                rb.AddForce(direction * m_reflectForce, ForceMode2D.Impulse);
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
        Gizmos.DrawWireSphere(transform.position, m_reflectRadius);
    }
}
