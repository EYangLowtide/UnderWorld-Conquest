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
        anim.SetBool("EnemyAttack", true);
        yield return new WaitForSeconds(0.1f); // Short delay to ensure the animation starts

        Collider2D[] playersToDamage = Physics2D.OverlapCircleAll(attackPos.position, attackRange, whatIsPlayer);
        foreach (Collider2D player in playersToDamage)
        {
            if (player != null)
            {
                Debug.Log("Player has been hit by enemy");
                //player.GetComponent<PlayerStats>().TakeDamage(damage);
            }
        }

        yield return new WaitForSeconds(0.1f); // Ensure the attack completes before resetting the bool
        anim.SetBool("EnemyAttack", false);
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(attackPos.position, attackRange);
    }
}
