using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    private Rigidbody2D rb;
    public EnemyScriptableObject enemyData;
    public Vector2 moveDir;  // Direction of movement
    private Transform player;
    private bool isCollidingWithPlayer = false;
    private bool isCollidingWithEnemy = false;
    private bool isCollidingWithEnvironment = false;
    private Vector2 collisionNormal;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Start()
    {
        player = FindObjectOfType<PlayerMovement>().transform;
    }

    void Update()
    {
        if (!isCollidingWithPlayer && !isCollidingWithEnemy && !isCollidingWithEnvironment)
        {
            MoveTowardsPlayer();
        }
        else if (isCollidingWithEnvironment)
        {
            AdjustMoveDirection();
        }
    }

    private void MoveTowardsPlayer()
    {
        if (player != null)
        {
            Vector2 targetPosition = player.transform.position;
            moveDir = (targetPosition - rb.position).normalized;
            rb.MovePosition(rb.position + moveDir * enemyData.MoveSpeed * Time.deltaTime);
        }
    }

    private void AdjustMoveDirection()
    {
        Vector2 adjustedMoveDir = moveDir;

        // Prevent movement into the collision normal
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

        rb.velocity = adjustedMoveDir * enemyData.MoveSpeed;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            isCollidingWithPlayer = true;
            HandlePlayerCollision(collision.gameObject);
            Debug.Log("Enemy has found player");
        }
        else if (collision.CompareTag("Enemy"))
        {
            isCollidingWithEnemy = true;
            HandleEnemyCollision(collision.gameObject);
            Debug.Log("Enemy has found another enemy");
        }
        else if (collision.CompareTag("Enviroment"))
        {
            isCollidingWithEnvironment = true;
            collisionNormal = (transform.position - collision.transform.position).normalized;
            Debug.Log("Enemy has found the environment");
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
        else if (collision.CompareTag("Enviroment"))
        {
            isCollidingWithEnvironment = false;
        }
    }

    private void HandlePlayerCollision(GameObject player)
    {
        Vector2 pushDirection = (player.transform.position - transform.position).normalized;
        rb.velocity = Vector2.zero; // Stop enemy movement
        player.GetComponent<Rigidbody2D>().velocity = Vector2.zero; // Stop player movement
        rb.AddForce(-pushDirection * 5f, ForceMode2D.Impulse); // Pushes the enemy away
        player.GetComponent<Rigidbody2D>().AddForce(pushDirection * 5f, ForceMode2D.Impulse); // Pushes the player away
    }

    private void HandleEnemyCollision(GameObject otherEnemy)
    {
        Vector2 pushDirection = (otherEnemy.transform.position - transform.position).normalized;
        rb.velocity = Vector2.zero; // Stop this enemy movement
        otherEnemy.GetComponent<Rigidbody2D>().velocity = Vector2.zero; // Stop other enemy movement
        rb.AddForce(-pushDirection * 2.5f, ForceMode2D.Impulse); // Pushes this enemy away with less force
        otherEnemy.GetComponent<Rigidbody2D>().AddForce(pushDirection * 2.5f, ForceMode2D.Impulse); // Pushes the other enemy away with less force
    }
}
