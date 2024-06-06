using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimator : MonoBehaviour
{
    //References
    Animator anim;
    PlayerMovement plyMov;
    SpriteRenderer sprtRend;

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        plyMov = GetComponent<PlayerMovement>();
        sprtRend = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if (plyMov.moveDir.x != 0 || plyMov.moveDir.y != 0)
        {
            anim.SetBool("isMoving", true);

            SpriteDirectionChecker();
        }
        else
        {
            anim.SetBool("isMoving", false);
        }
    }

    void SpriteDirectionChecker()
    {
        if (plyMov.lastHorizontalVector < 0)
        {
            sprtRend.flipX = true;
        }
        else
        {
            sprtRend.flipX = false;
        }
    }
}
