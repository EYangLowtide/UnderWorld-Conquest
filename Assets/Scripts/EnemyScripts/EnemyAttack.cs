using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttack : MonoBehaviour
{
    private float timeBtwAttack;
    public float startTimeBtwAttack = 1f;
    private Transform player;
    public Transform attackPos;
    public LayerMask whatIsPlayer;
    public float attackRange;
    public int damage;

    public Animator anim;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        timeBtwAttack = startTimeBtwAttack;
    }

    private void Update()
    {
        if (player == null)
            return;

        float distance = Vector2.Distance(player.position, transform.position);
        if (distance <= attackRange)
        {
            if (timeBtwAttack <= 0)
            {
                StartCoroutine(PerformAttack());
                timeBtwAttack = startTimeBtwAttack;
            }
            else
            {
                timeBtwAttack -= Time.deltaTime;
            }
        }
    }

    private IEnumerator PerformAttack()
    {
        anim.SetTrigger("EnemyAttack");
        yield return new WaitForSeconds(0.1f); // Short delay to ensure the animation starts

        Collider2D[] playersToDamage = Physics2D.OverlapCircleAll(attackPos.position, attackRange, whatIsPlayer); //waht is player is looking for layer not tag or sorting
        foreach (Collider2D playerCollider in playersToDamage)
        {
            if (playerCollider != null)
            {
                Debug.Log("Player has been hit by enemy");
                playerCollider.GetComponent<PlayerStats>().TakeDamage(damage);
            }
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(attackPos.position, attackRange);
    }
}
