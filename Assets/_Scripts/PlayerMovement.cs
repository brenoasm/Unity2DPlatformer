using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private const string CHARACTER_STATE = "characterState";

    private Rigidbody2D rb;
    private BoxCollider2D boxCollider2D;
    private Animator animator;
    private SpriteRenderer spriteRenderer;
    
    private float dirX = 0f;

    private bool hasDoubleJumped = false;

    [SerializeField] private float moveSpeed = 7f;
    [SerializeField] private float jumpForce = 14f;
    [SerializeField] private LayerMask jumpableGround;

    private CharacterState characterState = CharacterState.idle;

    [SerializeField] private AudioSource jumpSoundEffect;

    private enum CharacterState
    {
        idle = 0, running = 1, jumping = 2, falling = 3, doubleJumping = 4
    }

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        boxCollider2D = GetComponent<BoxCollider2D>();
    }

    private void Update()
    {
        if (rb.bodyType == RigidbodyType2D.Static)
        {
            return;
        }

        HandleInputs();
    }

    private void HandleInputs()
    {
        dirX = Input.GetAxisRaw("Horizontal");
        
        rb.velocity = new Vector2(dirX * moveSpeed, rb.velocity.y);

        if (Input.GetButtonDown("Jump")
            && (characterState == CharacterState.jumping
            || characterState == CharacterState.falling)
            && !hasDoubleJumped
            )
        {
            hasDoubleJumped = true;

            jumpSoundEffect.Play();

            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
        }

        if (Input.GetButtonDown("Jump") && IsGrounded())
        {
            hasDoubleJumped = false;
            jumpSoundEffect.Play();

            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
        }

        UpdateAnimation();
    }

    private void UpdateAnimation()
    {
        if (dirX > 0f)
        {
            spriteRenderer.flipX = false;
            characterState = CharacterState.running;
        }
        else if (dirX < 0f)
        {
            spriteRenderer.flipX = true;
            characterState = CharacterState.running;
        }
        else
        {
            characterState = CharacterState.idle;
        }

        if (rb.velocity.y > .1f)
        {
            if (hasDoubleJumped)
            {
                characterState = CharacterState.doubleJumping;
            } else
            {
                characterState = CharacterState.jumping;
            }
        }
        else if (rb.velocity.y < -.9f)
        {
            characterState = CharacterState.falling;
        }

        animator.SetInteger(CHARACTER_STATE, (int)characterState);
    }

    private bool IsGrounded()
    {
        return Physics2D.BoxCast(boxCollider2D.bounds.center, boxCollider2D.bounds.size,
            0f, Vector2.down, .1f, jumpableGround);
    }
}
