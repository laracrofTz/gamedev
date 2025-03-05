using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BloopaMovement : MonoBehaviour
{
    public float speed = 5.0f;
    public Transform topmostPlatform;
    private Rigidbody2D bloopaBody;
    private bool carryingMario = false;
    private bool returning = false;
    private GameObject mario;
    private Vector2 initialPosition;
    public Sprite bloopaIn;
    public Sprite bloopaOut;
    private SpriteRenderer spriteRenderer;
    public float stopAbovePlatformOffset = 1.0f;
    public Transform tentacleHolder;
    private AudioSource audioSource;
    private Animator animator;
    private RuntimeAnimatorController originalController;

    void Start()
    {
        bloopaBody = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        initialPosition = bloopaBody.position;
        spriteRenderer.sprite = bloopaIn;
        audioSource = GetComponent<AudioSource>();

        originalController = animator.runtimeAnimatorController;

        bloopaBody.gravityScale = 0f;
        bloopaBody.constraints = RigidbodyConstraints2D.FreezeRotation;

        animator.SetBool("IsIdle", true);
    }

    void FixedUpdate()
    {
        if (carryingMario || returning)
        {
            // disable animator while moving
            if (animator.enabled)
            {
                animator.enabled = false;
            }

            if (carryingMario)
            {
                MoveToTarget(new Vector2(bloopaBody.position.x, topmostPlatform.position.y + stopAbovePlatformOffset));

                if (mario != null && tentacleHolder != null)
                {
                    mario.transform.position = tentacleHolder.position;
                }

                float distanceToTarget = Mathf.Abs(bloopaBody.position.y - (topmostPlatform.position.y + stopAbovePlatformOffset));
                if (distanceToTarget < 0.1f)
                {
                    ReleaseMario();
                }
            }
            else // returning
            {
                MoveToTarget(initialPosition);

                // check to see if reached initial position
                if (Vector2.Distance(bloopaBody.position, initialPosition) < 0.1f)
                {
                    ReturnToIdle();
                }
            }
        }
        else
        {
            // enable animator during idle
            if (!animator.enabled)
            {
                animator.enabled = true;
                animator.SetBool("IsIdle", true);
            }
            bloopaBody.linearVelocity = Vector2.zero;
        }
    }

    private void MoveToTarget(Vector2 targetPosition)
    {
        Vector2 direction = (targetPosition - bloopaBody.position).normalized;
        bloopaBody.linearVelocity = direction * speed;
    }

    private void ReleaseMario()
    {
        carryingMario = false;
        if (mario != null)
        {
            mario.GetComponent<PlayerMovement>().SetBeingCarriedByBloopa(false, null);
        }
        returning = true;
        spriteRenderer.sprite = bloopaIn;
        audioSource.Stop();
    }

    private void ReturnToIdle()
    {
        returning = false;
        bloopaBody.linearVelocity = Vector2.zero;
        transform.position = initialPosition;
        spriteRenderer.sprite = bloopaIn;
        animator.enabled = true;
        animator.SetBool("IsIdle", true);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player") && !carryingMario && !returning)
        {
            carryingMario = true;
            mario = other.gameObject;

            mario.GetComponent<PlayerMovement>().SetBeingCarriedByBloopa(true, tentacleHolder.gameObject);
            spriteRenderer.sprite = bloopaOut;

            if (!audioSource.isPlaying)
            {
                audioSource.Play();
            }

            // disable animator when starting to carry
            animator.enabled = false;
        }
    }
}