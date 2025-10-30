
using System.IO;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed = 5f;
    public float jumpForce = 10f;

    [Header("Double Jump")]
    public bool canDoubleJump = true;
    public float doubleJumpForce = 8f;
    private int jumpsRemaining = 2;

    [Header("Combat")]
    public GameObject bulletPrefab;
    public Transform firePoint;

    [Header("Ground Check")]
    public Transform groundCheck;
    public LayerMask groundLayer;

    [Header("Components")]
    public Rigidbody2D rb;
    public Animator animator;
    public SpriteRenderer spriteRenderer;

    private PlayerState currentState;

    [Header("Dash")]
    public bool canDash = true;
    public float dashSpeed = 20f;
    public float dashDuration= 0.2f;
    public KeyCode dashKey = KeyCode.LeftShift;
    public float defaultGravityScale;

    private void Awake()
    {
        
    }

    void Start()
    {
        if (rb == null) rb = GetComponent<Rigidbody2D>();
        if (animator == null) animator = GetComponent<Animator>();
        if (spriteRenderer == null) spriteRenderer = GetComponent<SpriteRenderer>();

        ChangeState(new IdleState());

        Debug.Log("=== DOUBLE JUMP SETUP ===");
        Debug.Log("Can Double Jump: " + canDoubleJump);
        Debug.Log("Jumps Remaining: " + jumpsRemaining);
        defaultGravityScale = rb.gravityScale;
    }

    void Update()
    {
        if (GameManager.Instance != null && GameManager.Instance.IsPaused())
        {
            return;
        }

        // Reset jump count when grounded
        if (IsGrounded() && rb.linearVelocity.y <= 0)
        {
            int oldJumps = jumpsRemaining;
            jumpsRemaining = canDoubleJump ? 2 : 1;

            if (oldJumps != jumpsRemaining)
            {
                Debug.Log("Jumps Reset: " + jumpsRemaining + " (Grounded)");
            }
        }

        if (currentState != null)
        {
            currentState.UpdateState(this);
        }

       if(Input.GetKeyDown(dashKey) && canDash)
        {
            ChangeState(new DashingState());
        }
    }

    public void PerformDash()
    {
        if (!canDash)
        {
            return;
        }
        ChangeState(new DashingState());
    }

    public void ChangeState(PlayerState newState)
    {
        if (currentState != null)
        {
            currentState.ExitState(this);
        }

        currentState = newState;
        currentState.EnterState(this);

        EventManager.TriggerEvent("OnPlayerStateChanged", currentState.GetStateName());
    }

    public bool IsGrounded()
    {
        return Physics2D.OverlapCircle(groundCheck.position, 0.2f, groundLayer);
    }

    public bool CanJump()
    {
        bool can = jumpsRemaining > 0;
        Debug.Log("CanJump? " + can + " (Jumps: " + jumpsRemaining + ", Grounded: " + IsGrounded() + ")");
        return can;
    }

    public void PerformJump()
    {
        if (jumpsRemaining <= 0)
        {
            Debug.Log("Can't jump - no jumps left!");
            return;
        }

        jumpsRemaining--;

        // Use different force for double jump
        float force = (jumpsRemaining == 0 && canDoubleJump) ? doubleJumpForce : jumpForce;

        Vector2 velocity = rb.linearVelocity;
        velocity.y = force;
        rb.linearVelocity = velocity;

        Debug.Log("JUMPED! Jumps left: " + jumpsRemaining + " | Force: " + force);

        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.PlayJumpSound();
        }

        if (jumpsRemaining == 0 && canDoubleJump)
        {
            Debug.Log("DOUBLE JUMP!");
            EventManager.TriggerEvent("OnDoubleJump");
        }
    }

    public int GetJumpsRemaining()
    {
        return jumpsRemaining;
    }

    public void Fire()
    {
        if (bulletPrefab != null && firePoint != null)
        {
            Instantiate(bulletPrefab, firePoint.position, Quaternion.identity);
            AudioManager.Instance.PlayShootSound();
        }
    }

    public void TakeDamage()
    {
        GameManager.Instance.PlayerDied();
        Respawn();
    }

    void Respawn()
    {
        transform.position = GameManager.Instance.spawnPoint;
        jumpsRemaining = canDoubleJump ? 2 : 1;
        ChangeState(new IdleState());
        rb.linearVelocity = Vector2.zero;
    }

    public string GetCurrentStateName()
    {
        return currentState != null ? currentState.GetStateName() : "None";
    }
}