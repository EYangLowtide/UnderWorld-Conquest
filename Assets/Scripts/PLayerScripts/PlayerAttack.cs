using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    private float timeBtwAttack;
    public float startTimeBtwAttack;

    public Transform attackPos;
    public LayerMask whatIsEnemy;
    public float attackRangeX;
    public float attackRangeY;

    public int damage;

    public Animator anim;



    private void Update()
    {
        
        if(Input.GetKeyDown(KeyCode.Space))
            {
                anim.SetTrigger("MeleeAttack");
                Collider2D[] enemyToDamage = Physics2D.OverlapBoxAll(attackPos.position, new Vector2( attackRangeX, attackRangeY), 3, whatIsEnemy, 5);
                for (int i = 0; i < enemyToDamage.Length; i++)
                {
                    enemyToDamage[i].GetComponent<Enemy>().TakeDamage(damage);
                }
            }
    
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(attackPos.position, new Vector2(attackRangeX, attackRangeY));

    }

}
