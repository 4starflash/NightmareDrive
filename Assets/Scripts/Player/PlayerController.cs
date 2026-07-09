using System;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private PlayerData playerData;
    [SerializeField] private Rigidbody2D rb;

    private Vector2 m_movementInput;

    private void Update()
    {
        m_movementInput.x = Input.GetAxisRaw("Horizontal");
        m_movementInput.y = Input.GetAxisRaw("Vertical");

        m_movementInput = m_movementInput.normalized;
    }

    private void FixedUpdate()
    {
        rb.velocity = new Vector2(m_movementInput.x * playerData.playerDataClass.playerSpeed, m_movementInput.y * playerData.playerDataClass.playerSpeed);
    }
}
