using System.Collections;
using UnityEngine;

public class FlyingEnemyAttack : MonoBehaviour
{
    [Header("References")]
    public Transform player;
    public Transform bulletSpawnPoint;
    public GameObject bulletPrefab;
    private SpriteRenderer spriteRenderer;

    [Header("Attack Settings")]
    public float attackCooldown = 2f;
    private float attackTimer;
    public float bulletSpeed = 5f;

    [Header("Damage Settings")]
    public Color damageColor = Color.yellow;
    public float flashDuration = 0.25f;
    private Color originalColor;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteRenderer != null)
        {
            originalColor = spriteRenderer.color;
        }
    }

    private void Update()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        if (player == null) return;

        HandleAttack();
    }

    private void HandleAttack()
    {
        attackTimer -= Time.deltaTime;

        if (attackTimer <= 0f)
        {
            ShootAtPlayer();
            attackTimer = attackCooldown;
        }
    }

    private void ShootAtPlayer()
    {
        if (player == null || bulletSpawnPoint == null || bulletPrefab == null) return;

        Vector2 direction = (player.position - bulletSpawnPoint.position).normalized;

        GameObject bullet = Instantiate(bulletPrefab, bulletSpawnPoint.position, Quaternion.identity);

        Rigidbody2D bulletRb = bullet.GetComponent<Rigidbody2D>();
        if (bulletRb != null)
        {
            bulletRb.velocity = direction * bulletSpeed;
        }

        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        bullet.transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle - 90));
    }

    public void FlashDamage()
    {
        StartCoroutine(FlashDamageCoroutine());
    }

    private IEnumerator FlashDamageCoroutine()
    {
        if (spriteRenderer == null) yield break;

        spriteRenderer.color = damageColor;
        yield return new WaitForSeconds(flashDuration);
        spriteRenderer.color = originalColor;
    }
}
