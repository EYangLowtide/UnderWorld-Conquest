using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DemonBullet : MonoBehaviour
{
    public float speed;
    public float damage;

    private Vector2 moveDirection;

    // Set the bullet's direction and rotation on initialization
    public void Initialize(float bulletSpeed, float bulletDamage, Vector2 direction)
    {
        speed = bulletSpeed;
        damage = bulletDamage;
        moveDirection = direction;

        // Rotate the bullet based on the direction
        if (direction == Vector2.right)
        {
            transform.rotation = Quaternion.Euler(0, 0, 0); // Facing right
        }
        else if (direction == Vector2.left)
        {
            transform.rotation = Quaternion.Euler(0, 0, 180); // Facing left
        }
    }


    void Update()
    {
        // Move the bullet in the set direction
        transform.Translate(Vector2.up * speed * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            PlayerStats playerStats = collision.GetComponent<PlayerStats>();
            if (playerStats != null)
            {
                playerStats.TakeDamage(damage);
            }
            Destroy(gameObject);
        }
        else if (collision.CompareTag("Environment"))
        {
            Destroy(gameObject);
        }
    }
}
