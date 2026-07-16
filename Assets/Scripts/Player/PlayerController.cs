using System;
using UnityEngine;

public enum PlayerState { Idle, Running, Reflecting, Attacking, Dead }

public class PlayerController : MonoBehaviour
{
    [Header("Player Components")]
    [SerializeField] private PlayerData m_playerData;
    [SerializeField] private Rigidbody2D m_rb;
    [SerializeField] private Animator m_anim;
    [SerializeField] private GameObject m_ultHitbox;

    [Header("Reflect Ability Settings")]
    [SerializeField] private float m_reflectForce = 20f;
    [SerializeField] private float m_reflectRadius = 3f;
    [SerializeField] private LayerMask m_reflectableLayer;

    [Header("Ult Settings")]
    [SerializeField] private float m_ultForce = 5f;

    [Header("Player Bullet")]
    [SerializeField] private GameObject m_leftFacingBullet;
    [SerializeField] private GameObject m_rightFacingBullet;
    [SerializeField] private Transform m_firepoint;
    private float m_timer;
    [SerializeField] private float m_firingRate = 0.2f;

    private Vector2 m_movementInput;
    private bool m_flipped;

    public static Action<float> OnHealthChange;
    public static Action<float> OnEnergyChange;

    private void Start()
    {
        m_playerData.currentHealth = m_playerData.maxHealth;
        m_playerData.currentEnergy = m_playerData.maxEnergy;
        SetCurrentState(PlayerState.Idle);
    }

    private void Update()
    {
        if(m_playerData.currentState != PlayerState.Dead)
        {
            m_movementInput.x = Input.GetAxisRaw("Horizontal");
            m_movementInput.y = Input.GetAxisRaw("Vertical");
            m_movementInput = m_movementInput.normalized;

            if (m_rb.velocity != Vector2.zero && m_playerData.currentState != PlayerState.Attacking && m_playerData.currentState != PlayerState.Reflecting)
            {
                SetCurrentState(PlayerState.Running);
            }
            else if (m_rb.velocity == Vector2.zero && m_playerData.currentState != PlayerState.Attacking && m_playerData.currentState != PlayerState.Reflecting)
            {
                SetCurrentState(PlayerState.Idle);
            }

            // Flip player when turning
            if (m_movementInput.x > 0 && m_flipped && m_playerData.currentState != PlayerState.Attacking)
            {
                FlipPlayer();
            }
            else if (m_movementInput.x < 0 && !m_flipped && m_playerData.currentState != PlayerState.Attacking)
            {
                FlipPlayer();
            }

            // X to reflect or parry
            if (Input.GetKeyDown(KeyCode.X) && m_playerData.currentState != PlayerState.Reflecting && m_playerData.currentState != PlayerState.Attacking)
            {
                SetCurrentState(PlayerState.Reflecting);
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
            if (Input.GetKeyDown(KeyCode.C) && m_playerData.currentState != PlayerState.Attacking && m_playerData.currentState != PlayerState.Reflecting && m_playerData.currentEnergy >= m_playerData.maxEnergy)
            {
                SetCurrentState(PlayerState.Attacking);
                UpdateEnergy(-m_playerData.maxEnergy);
            }
        }
    }

    private void FixedUpdate()
    {
        if(m_playerData.currentState != PlayerState.Attacking && m_playerData.currentState != PlayerState.Dead)
        {
            m_rb.velocity = new Vector2(m_movementInput.x * m_playerData.playerSpeed, m_movementInput.y * m_playerData.playerSpeed);
        }
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
            case PlayerState.Reflecting:
                m_anim.SetTrigger("reflect");
                m_rb.velocity = Vector2.zero;
                break;
            case PlayerState.Attacking:
                m_anim.SetTrigger("melee");
                m_rb.velocity = Vector2.zero;
                break;
            case PlayerState.Dead:
                PlayerDeath();
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

    private void UpdateHealth(int value)
    {
        if (m_playerData.currentHealth - value < 0)
        {
            m_playerData.currentHealth = 0;
        }
        else
        {
            m_playerData.currentHealth -= value;
        }

        float currentRatio = m_playerData.currentHealth / m_playerData.maxHealth;
        OnHealthChange?.Invoke(currentRatio);

        if (m_playerData.currentHealth <= 0)
        {
            SetCurrentState(PlayerState.Dead);
        }
    }

    private void UpdateEnergy(float value)
    {
        if(m_playerData.currentEnergy + value > m_playerData.maxEnergy)
        {
            m_playerData.currentEnergy = m_playerData.maxEnergy;
        }
        else
        {
            m_playerData.currentEnergy += value;
        }

        float currentRatio = m_playerData.currentEnergy / m_playerData.maxEnergy;
        OnEnergyChange?.Invoke(currentRatio);
    }

    private void PlayerDeath()
    {
        m_anim.SetBool("dead", true);
        m_anim.SetBool("run", false);
        m_rb.velocity = Vector2.zero;
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.CompareTag("DamageSource") && m_playerData.currentState != PlayerState.Attacking)
        {
            UpdateHealth(10);
        }
    }

    private void ToggleUltHitbox()
    {
        m_ultHitbox.SetActive(!m_ultHitbox.activeSelf);
    }

    private void UltKnockback()
    {
        m_rb.AddForce(-transform.right * m_ultForce, ForceMode2D.Impulse);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position, m_reflectRadius);
    }
}
