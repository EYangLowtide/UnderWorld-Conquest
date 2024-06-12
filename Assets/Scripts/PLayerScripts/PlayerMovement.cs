using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{

    //Movement
    public float moveSpeed;

    Rigidbody2D rigBod;
    
    [HideInInspector]
    public float lastHorizontalVector;
    [HideInInspector]
    public float lastVerticalVector;
    [HideInInspector]
    public Vector2 moveDir;
    [HideInInspector]
    public Vector2 lastMovedVector;

    // Start is called before the first frame update
    void Start()
    {
        rigBod = GetComponent<Rigidbody2D>();
        lastMovedVector = new Vector2(1, 0f); //
    }

    // Update is called once per frame
    void Update()
    {
        InputManagement();
    }

    private void FixedUpdate()
    {
        Move();
    }

    void InputManagement()
    {
        float moveX = Input.GetAxisRaw("Horizontal");
        float moveY = Input.GetAxisRaw("Vertical");

        moveDir = new Vector2 (moveX, moveY).normalized;

        if (moveDir.x != 0)
        {
            lastHorizontalVector = moveDir.x;
            lastMovedVector = new Vector2(lastHorizontalVector, 0f); // last move x
        }

        if (moveDir.y != 0)
        {
            lastVerticalVector = moveDir.y;
            lastMovedVector = new Vector2(0f, lastVerticalVector); // last moved y 
        }

        if(moveDir.x !=0 && moveDir.y !=0)
        {
            lastMovedVector = new Vector2(lastHorizontalVector, lastVerticalVector); // while moving 
        }
    }

    void Move()
    {
        rigBod.velocity = new Vector2 (moveDir.x * moveSpeed, moveDir.y * moveSpeed);
    }


}
