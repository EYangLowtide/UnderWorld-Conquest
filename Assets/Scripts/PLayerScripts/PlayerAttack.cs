using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    private float timeBtwAttack;
    public float startTimeBtwAttack;

    public Transform attackPos;
    public LayerMask whatIsEnemy;
    public LayerMask whatIsProps;
    public float attackRange;
    public float meleeDamage;

    public Animator anim;

    public float GetCurrentMeleeDamage()
    {
        return meleeDamage *= FindObjectOfType<PlayerStats>().currentGuts;
    }
    private void Update()
    {
        if (timeBtwAttack <= 0)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                anim.SetTrigger("MeleeAttack");
                Collider2D[] enemiesToDamage = Physics2D.OverlapCircleAll(attackPos.position, attackRange, whatIsEnemy);
                Collider2D[] propsToDamage = Physics2D.OverlapCircleAll(attackPos.position, attackRange, whatIsProps);

                foreach (Collider2D enemy in enemiesToDamage)
                {
                    if (enemy != null)
                    {
                        Debug.Log("Enemy has been hit by player");
                        enemy.GetComponent<EnemyStats>().TakeDamage(GetCurrentMeleeDamage());
                    }
                }

                foreach (Collider2D prop in propsToDamage)
                {
                    if (prop != null)
                    {
                        Debug.Log("Prop has been hit by player");
                        if (prop.gameObject.TryGetComponent(out BreakableProps breakable))
                        {
                            breakable.TakeDamage(GetCurrentMeleeDamage());
                        }
                    }
                }

                timeBtwAttack = startTimeBtwAttack;
            }
        }
        else
        {
            timeBtwAttack -= Time.deltaTime;
        }
    }

    /* 
    The OnTriggerEnter2D method works but causes player to damage without key press
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            anim.SetTrigger("MeleeAttack");
            collision.GetComponent<Enemy>().TakeDamage(damage);
            timeBtwAttack = startTimeBtwAttack;
        }
    }
    */

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(attackPos.position, attackRange);
    }
}
