using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerJumping : MonoBehaviour
{
    public PlayerController player;

    public Rigidbody2D rb;

    public float jumpHeight = 5f;
    public float jumpTime = 1f;
    public float gravity = 1f;
    float jumpTimer = float.MaxValue;
    public AnimationCurve jumpUpCurve;

    public bool jumping = false;

    float initialY;

    [Header("Collision Detection")]
    public float rayCircleRadius = 0.2f;
    public float downOffset = 1f;
    public Collider2D downCollider;

    // Update is called once per frame
    void Update()
    {
        ManageCollisions();

        if (!jumping)
        {
            if (Input.GetButtonDown("Jump")
            && CanJump())
            {
                Jump();
            }
        }

    }

    private void FixedUpdate()
    {
        ManageJump();
    }

    private void ManageCollisions()
    {
        // Abajo
        downCollider = null;
        Collider2D[] downColliders = Physics2D.OverlapCircleAll(rb.transform.position + Vector3.down * downOffset, rayCircleRadius);
        foreach (Collider2D c in downColliders)
        {
            if (c.CompareTag("Ground"))
            {
                downCollider = c;
            }
        }
    }

    private void ManageJump()
    {
        // Saltar
        if ((jumping && jumpTimer < jumpTime))
        {
            Debug.Log("GOING UP");
            float yPos = initialY + jumpUpCurve.Evaluate(jumpTimer / jumpTime) * jumpHeight;
            // TODO: Usar velocity
            rb.transform.position = new Vector3(rb.transform.position.x, yPos, rb.transform.position.z);
            jumpTimer += Time.fixedDeltaTime;
        }

        // Usar gravedad
        if ((jumping && jumpTimer >= jumpTime))
        {
            Debug.Log("GOING DOWN");
            //FinishJump();
            rb.velocity += Vector2.down * gravity * 10f * Time.fixedDeltaTime;
        }

        // Caer
        if (!jumping && downCollider == null)
        {
            player.currentState = PlayerState.jumping;
            jumpTimer = float.MaxValue;
            jumping = true;
        }
    }

    

    private bool CanJump()
    {
        return player.currentState != PlayerState.jumping;
    }

    private void Jump()
    {
        //Debug.Log("Aplicando un salto con fuerza " + jumpForce);
        player.currentState = PlayerState.jumping;
        //rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
        initialY = rb.transform.position.y;
        jumpTimer = 0f;
        jumping = true;
    }

    private void FinishJump()
    {
        player.currentState = PlayerState.idle;
        jumpTimer = float.MaxValue;
        jumping = false;
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        // Si el personaje tocó el piso
        // TODO: Detectar que lo toque con los pies usando raycast
        if (jumping && other.gameObject.CompareTag("Ground"))
        {
            FinishJump();    
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(rb.transform.position + Vector3.down * downOffset, rayCircleRadius);
    }
}
