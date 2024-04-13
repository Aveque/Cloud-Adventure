using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class Hero : Entity
{
    private float horizontal;
    private bool isFacingRight = true;
    private bool isWallSliding;
    private float WallSlidingSpeed = 2.15f;
    [SerializeField] private Transform wallCheck;

    [SerializeField] private int lives = 999;
    [SerializeField] private float jumpForce = 15f;
    [SerializeField] private bool isGrounded = false;
    [SerializeField] private float maxSpeed = 10f;
    [SerializeField] BoxCollider2D bcl2d;
    [SerializeField] private LayerMask layer;
    private Rigidbody2D rb;
    private SpriteRenderer sprite;
    private Animator anim;
    [SerializeField] private ParticleSystem dust;
    [SerializeField] private ParticleSystem dust2;
    bool canDoubleJump;

    private bool canDash = true;
    private bool isDashing;
    private float dashingPower = 30f;
    private float dashingTime = 0.2f;
    private float dashingCooldown = 1f;

    private bool isWallJumping;
    private float wallJumpingDirection;
    private float wallJumpingTime = 0.2f;
    private float wallJumpingCounter;
    private float wallJumpingDuration = 0.37f;
    private Vector2 wallJumpingPower = new Vector2(8f, 16f);

    public Joystick joystick;
    private bool JumpButton;

    private void FixedUpdate()
    {
        if (isDashing)
        {
            return;
        }

        CheckGround();

        if (!isWallJumping)
        {
            rb.velocity = new Vector2(horizontal * 8f, rb.velocity.y);
        }
    }
    private States State
    {
        get { return (States)anim.GetInteger("state"); }
        set { anim.SetInteger("state", (int)value); }
    }
    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        sprite = GetComponentInChildren<SpriteRenderer>();
        anim = GetComponent<Animator>();
        Instance = this;
    }

    void Update()
    {
        if (isDashing)
        {
            return;
        }


        if (isGrounded)
        {
            Run();
        }

        horizontal = joystick.Horizontal;
        if (isGrounded) State = States.idle;

        if (GetJumpButton())
        {
            rb.gravityScale = 4f;
        }
        else
        {
            rb.gravityScale = 8;
        }

        WallSlide();
        WallJump();

        if (!isWallJumping)
        {
            Flip();
        }
    }

    private void Run()
    {
        if (isGrounded) State = States.run;

        Vector3 dir = new Vector3(joystick.Horizontal, 0.0f, 0.0f);
        bool joyst;
        if(joystick.Horizontal != 0)
        {
            joyst = true;
        }
        else
        {
            joyst = false;
        }
        if (joyst)
        {
            rb.velocity = new Vector2(maxSpeed * dir.x, rb.velocity.y);
            if (dir.x < 0 && isFacingRight)
            {
                Vector3 localScale = transform.localScale;
                localScale.x *= -1f;
                transform.localScale = localScale;
                isFacingRight = false;
            }
            if (dir.x > 0 && !isFacingRight)
            {
                Vector3 localScale = transform.localScale;
                localScale.x *= -1f;
                transform.localScale = localScale;
                isFacingRight = true;
            }
        }
        else
        {
            rb.velocity = new Vector2(0, rb.velocity.y);
        }
    }
    
    public void OnJumpButtonDown()
    {
        if (isGrounded)
        {
            dust.Play();
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
        }
        else
        {
            if (canDoubleJump)
            {
                dust.Play();
                rb.velocity = new Vector2(rb.velocity.x, jumpForce);
                canDoubleJump = false;
            }
        }

    }

    public bool GetJumpButton()
    {
        return JumpButton;
    }

    public void GetJumpButtonUp()
    {
        JumpButton = false;
    }
    public void GetJumpButtonDown()
    {
        JumpButton = true; 
    }

    private void Jump()
    {
        if (isGrounded)
        {
            dust.Play();
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
        }
        else
        {
            if (canDoubleJump)
            {
                dust.Play();
                rb.velocity = new Vector2(rb.velocity.x, jumpForce);
                canDoubleJump = false;
            }
        }
        
    }

    public void Dashing()
    {
        if (canDash)
        {
            StartCoroutine(Dash());
        }
    }
    
    private void CheckGround()
    {
        
        RaycastHit2D raycastHit2D = Physics2D.BoxCast(bcl2d.bounds.center, bcl2d.bounds.size, 0f, Vector2.down, 0.5f, layer);
        if(raycastHit2D.collider != null )
        {
            isGrounded = true;
            canDoubleJump = true;
        }
        else
        {
            isGrounded= false;
        }
        if (!isGrounded) State = States.jump;
    }

    private bool IsWalled()
    {
        return Physics2D.OverlapCircle(wallCheck.position, 0.2f, layer);
    }

    private void WallSlide()
    {
        if(IsWalled() && !isGrounded && horizontal != 0f)
        {
            isWallSliding = true;
            rb.velocity = new Vector2(rb.velocity.x, Mathf.Clamp(rb.velocity.y, -WallSlidingSpeed,float.MaxValue));
        }
        else
        {
            isWallSliding = false;
        }
    }
    private void WallJump()
    {
        if (isWallSliding)
        {
            isWallJumping = false;
            wallJumpingDirection = -transform.localScale.x;
            wallJumpingCounter = wallJumpingTime;

            CancelInvoke(nameof(StopWallJumping));
        }
        else
        {
            wallJumpingCounter -= Time.deltaTime;
        }

        if (GetJumpButton() && wallJumpingCounter > 0f)
        {
            isWallJumping = true;
            dust.Play();
            rb.velocity = new Vector2(wallJumpingDirection * wallJumpingPower.x, wallJumpingPower.y);
            wallJumpingCounter = 0f;

            if (transform.localScale.x != wallJumpingDirection)
            {
                isFacingRight = !isFacingRight;
                Vector3 localScale = transform.localScale;
                localScale.x *= -1f;
                transform.localScale = localScale;
            }

            Invoke(nameof(StopWallJumping), wallJumpingDuration);
        }

    }
    private void Flip()
    {
        if (isFacingRight && horizontal < 0f || !isFacingRight && horizontal > 0f)
        {
            isFacingRight = !isFacingRight;
            Vector3 localScale = transform.localScale;
            localScale.x *= -1f;
            transform.localScale = localScale;
        }
    }
    private void StopWallJumping()
    {
        isWallJumping = false;
    }

    public static Hero Instance { get; set; }

    public override void GetDamage()
    {
        lives -= 1;
        Debug.Log(lives);
        if(lives < 1)
        {
            Die();
        }
    }
    private IEnumerator Dash()
    {
        canDash = false;
        isDashing = true;
        float originalGravity = rb.gravityScale;
        rb.gravityScale = 0f;
        rb.velocity = new Vector2(transform.localScale.x * dashingPower, 0f);
        dust2.Play();
        yield return new WaitForSeconds(dashingTime);
        rb.gravityScale = originalGravity;
        isDashing = false;
        yield return new WaitForSeconds(dashingCooldown);
        canDash = true;
    }
}


public enum States
{
    idle,
    run,
    jump
}
