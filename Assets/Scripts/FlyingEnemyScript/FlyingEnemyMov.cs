using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SHG.AnimatorCoder;

public class FlyingEnemyMov : AnimatorCoder
{
    public static FlyingEnemyMov instance;

    public Transform player; // Reference to the player
    private Rigidbody2D rb;
    private SpriteRenderer sprite;
    private EnemyStats enemy; // Reference to enemy stats
    private Vector2 moveDir; // Movement direction
    private bool isCollidingWithPlayer = false;
    private bool isCollidingWithEnemy = false;
    private bool isCollidingWithEnvironment = false;
    private Vector2 collisionNormal;

    public float stopDistance = 5f; // Distance to stop from the player

    private readonly AnimationData IDLE = new(Animations.IDLE);
    private readonly AnimationData ATTACK = new(Animations.ATTACK1, true, new());

    private void Awake()
    {
        instance = this;
        rb = GetComponent<Rigidbody2D>();
        sprite = GetComponent<SpriteRenderer>();
        player = FindObjectOfType<PlayerMovement>().transform;
        enemy = GetComponent<EnemyStats>();
    }

    private void Update()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;

        if (!isCollidingWithPlayer && !isCollidingWithEnemy && !isCollidingWithEnvironment)
        {
            MoveTowardsPlayer();
        }
        else if (isCollidingWithEnvironment)
        {
            AdjustMoveDirection();
        }

        DefaultAnimation(0);
    }

    public override void DefaultAnimation(int layer)
    {
        Play(IDLE);
    }

    private void MoveTowardsPlayer()
    {
        if (player != null)
        {
            Vector2 targetPosition = player.position;
            float distanceToPlayer = Vector2.Distance(rb.position, targetPosition);

            // Stop moving if within the stopDistance
            if (distanceToPlayer > stopDistance)
            {
                moveDir = (targetPosition - rb.position).normalized;

                // Flip the sprite to face the player
                FlipSpriteToFacePlayer(moveDir);

                // Move towards the player without rotating
                rb.MovePosition(rb.position + moveDir * enemy.currentMoveSpeed * Time.deltaTime);
            }
            else
            {
                // If within stopDistance, ensure the enemy faces the player but doesn't move
                FlipSpriteToFacePlayer(targetPosition - rb.position);
                rb.velocity = Vector2.zero; // Explicitly stop the enemy
            }
        }
    }

    private void FlipSpriteToFacePlayer(Vector2 direction)
    {
        // Flip the sprite based on the horizontal direction
        if (direction.x > 0)
        {
            sprite.flipX = false; // Facing right
        }
        else
        {
            sprite.flipX = true; // Facing left
        }
    }

    private void AdjustMoveDirection()
    {
        Vector2 adjustedMoveDir = moveDir;

        if (Vector2.Dot(moveDir, collisionNormal) < 0)
        {
            if (Mathf.Abs(collisionNormal.x) > Mathf.Abs(collisionNormal.y))
            {
                adjustedMoveDir.x = 0;
            }
            else
            {
                adjustedMoveDir.y = 0;
            }
        }

        rb.velocity = adjustedMoveDir * enemy.currentMoveSpeed;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            isCollidingWithPlayer = true;
            HandlePlayerCollision(collision.gameObject);
        }
        else if (collision.CompareTag("Enemy"))
        {
            isCollidingWithEnemy = true;
            HandleEnemyCollision(collision.gameObject);
        }
        else if (collision.CompareTag("Environment"))
        {
            isCollidingWithEnvironment = true;
            collisionNormal = (transform.position - collision.transform.position).normalized;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            isCollidingWithPlayer = false;
        }
        else if (collision.CompareTag("Enemy"))
        {
            isCollidingWithEnemy = false;
        }
        else if (collision.CompareTag("Environment"))
        {
            isCollidingWithEnvironment = false;
        }
    }

    private void HandlePlayerCollision(GameObject player)
    {
        Vector2 pushDirection = (player.transform.position - transform.position).normalized;
        rb.velocity = Vector2.zero;
        player.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        rb.AddForce(-pushDirection * 5f, ForceMode2D.Impulse);
        player.GetComponent<Rigidbody2D>().AddForce(pushDirection * 5f, ForceMode2D.Impulse);
    }

    private void HandleEnemyCollision(GameObject otherEnemy)
    {
        Vector2 pushDirection = (otherEnemy.transform.position - transform.position).normalized;
        rb.velocity = Vector2.zero;
        otherEnemy.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        rb.AddForce(-pushDirection * 2.5f, ForceMode2D.Impulse);
        otherEnemy.GetComponent<Rigidbody2D>().AddForce(pushDirection * 2.5f, ForceMode2D.Impulse);
    }
}
