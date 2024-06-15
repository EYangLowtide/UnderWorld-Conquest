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
        if (!isCollidingWithPlayer)
        {
            MoveTowardsPlayer();
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

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            isCollidingWithPlayer = true;
            HandleCollision(collision.gameObject);
            Debug.Log("Enemy has found player");
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            isCollidingWithPlayer = false;
        }
    }

    private void HandleCollision(GameObject player)
    {
        Vector2 pushDirection = (player.transform.position - transform.position).normalized;
        rb.velocity = Vector2.zero; // Stop enemy movement
        player.GetComponent<Rigidbody2D>().velocity = Vector2.zero; // Stop player movement
        rb.AddForce(-pushDirection * 5f, ForceMode2D.Impulse); // Pushes the enemy away
        player.GetComponent<Rigidbody2D>().AddForce(pushDirection * 5f, ForceMode2D.Impulse); // Pushes the player away
    }
}
