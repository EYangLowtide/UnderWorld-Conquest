using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttack : MonoBehaviour
{
    private float timeBtwAttack;
    public float startTimeBtwAttack;

    public Transform attackPos;
    public LayerMask whatIsPlayer;
    public float attackRange;
    public int damage;

    public Animator anim;

    private void Update()
    {
        if (timeBtwAttack <= 0)
        {
            if (PlayerInAttackRange())
            {
                anim.SetTrigger("EnemyAttack");
                /*
                Collider2D[] playersToDamage = Physics2D.OverlapCircleAll(attackPos.position, attackRange, whatIsPlayer);
                foreach (Collider2D player in playersToDamage)
                {
                    if (player != null)
                    {
                        player.GetComponent<player>().TakeDamage(damage);
                    }
                }
                */
                timeBtwAttack = startTimeBtwAttack;
            }
        }
        else
        {
            timeBtwAttack -= Time.deltaTime;
        }
    }

    bool PlayerInAttackRange()
    {
        Collider2D player = Physics2D.OverlapCircle(attackPos.position, attackRange, whatIsPlayer);
        return player != null;
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(attackPos.position, attackRange);
    }
}
