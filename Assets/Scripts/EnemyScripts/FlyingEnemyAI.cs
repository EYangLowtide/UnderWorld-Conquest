using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyingEnemyAttack : MonoBehaviour
{
    [Header("References")]
    public Transform player; // Reference to the player
    public Transform bulletSpawnPoint; // Position where bullets spawn
    public GameObject bulletPrefab; // Bullet prefab

    [Header("Attack Settings")]
    public float attackCooldown = 2f; // Cooldown time between attacks
    private float attackTimer; // Tracks cooldown time
    public float bulletSpeed = 5f; // Speed of the bullet

    private void Update()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        if (player == null) return;

        HandleAttack();
    }

    private void HandleAttack()
    {
        attackTimer -= Time.deltaTime;

        // Attack if the cooldown is complete
        if (attackTimer <= 0f)
        {
            ShootAtPlayer();
            attackTimer = attackCooldown; // Reset cooldown
        }
    }

    private void ShootAtPlayer()
    {
        if (player == null || bulletSpawnPoint == null || bulletPrefab == null) return;

        // Calculate the direction from the bullet spawn point to the player
        Vector2 direction = (player.position - bulletSpawnPoint.position).normalized;

        // Instantiate the bullet
        GameObject bullet = Instantiate(bulletPrefab, bulletSpawnPoint.position, Quaternion.identity);

        // Set bullet velocity
        Rigidbody2D bulletRb = bullet.GetComponent<Rigidbody2D>();
        if (bulletRb != null)
        {
            bulletRb.velocity = direction * bulletSpeed; // Move the bullet in the calculated direction
        }

        // Rotate the bullet to face the direction it's moving
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        bullet.transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle - 90));
    }

    private void OnDrawGizmosSelected()
    {
        // Draw a line to the player to visualize aiming
        if (player != null)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawLine(bulletSpawnPoint.position, player.position);
        }
    }
}
