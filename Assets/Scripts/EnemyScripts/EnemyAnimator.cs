using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAnimator : MonoBehaviour
{
    // References
    Animator anim;
    EnemyMovement enmyMov;
    SpriteRenderer sprtRend;

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        enmyMov = GetComponent<EnemyMovement>();
        sprtRend = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if (enmyMov.moveDir.x != 0 || enmyMov.moveDir.y != 0)
        {
            anim.SetBool("EnemyIsMoving", true);
            SpriteDirectionChecker();
        }
        else
        {
            anim.SetBool("EnemyIsMoving", false);
        }
    }

    void SpriteDirectionChecker()
    {
        if (enmyMov.moveDir.x < 0)  // Check the x direction of moveDir
        {
            sprtRend.flipX = true;
        }
        else
        {
            sprtRend.flipX = false;
        }
    }
}
