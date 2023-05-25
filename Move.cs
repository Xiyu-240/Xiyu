using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Move : MonoBehaviour
{
    //public Life g;

    private Rigidbody2D rb;
    private SpriteRenderer rbSprite;
    private CapsuleCollider2D coll;
    private Animator animator;

    [SerializeField] private LayerMask Canjump;

    [SerializeField] private AudioSource jumpSuond;

    public float force;
    public float speed;
    private float dirX;

    private enum MovementState { idle, run, jump, fall }
    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rbSprite = GetComponent<SpriteRenderer>();
        coll = GetComponent<CapsuleCollider2D>();
        animator = GetComponent<Animator>();
        //Life g=GetComponent<Life>();
    }
    private void Update()
    {
        dirX = Input.GetAxisRaw("Horizontal");
        rb.velocity = new Vector3(dirX * speed, rb.velocity.y, 0);

        if (Input.GetKeyDown(KeyCode.Space) && IsGrounded())
        {
            jumpSuond.Play();
            rb.velocity = new Vector3(rb.velocity.x, force, 0);
        }

        UpdateAnimationState();
    }

    private void UpdateAnimationState()
    {
        MovementState state;

        if(dirX > 0f)
        {
            state = MovementState.run;
            rbSprite.flipX = false;
        }
        else if(dirX < 0f)
        {
            state = MovementState.run;
            rbSprite.flipX = true;
        }
        else
        {
            state = MovementState.idle;
        }

        if(rb.velocity.y > 0.1f)
        {
            state = MovementState.jump;
        }
        else if (rb.velocity.y < -0.1f)
        {
            state = MovementState.fall;
        }

        animator.SetInteger("state",(int)state);
    }

    private bool IsGrounded()
    {
        return Physics2D.BoxCast(coll.bounds.center, coll.bounds.size, 0f, Vector2.down, .1f, Canjump);
    }

   
   /* private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.layer!=LayerMask.NameToLayer("Enemy"))
        {
            return;
        }
        g.Die();
    }*/
}