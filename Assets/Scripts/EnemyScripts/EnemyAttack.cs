using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EnemyAttack : MonoBehaviour
{
    private float timeBtwAttack;
    public float startTimeBtwAttack;
    private Transform player;
    public Transform attackPos;
    public LayerMask whatIsPlayer;
    public float attackRange;
    public int damage;

    public Animator anim;

    private void Update()
    {
        if (player != null)
            return;
        float distance = Vector2.Distance(player.position, transform.position);
        if (distance <= attackRange)
        {
            anim.SetTrigger("EnemyAttack");
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(attackPos.position, attackRange);
    }
}
