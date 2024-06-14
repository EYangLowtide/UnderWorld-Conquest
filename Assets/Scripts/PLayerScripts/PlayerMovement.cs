using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [HideInInspector]
    public float lastHorizontalVector;
    [HideInInspector]
    public float lastVerticalVector;
    [HideInInspector]
    public Vector2 moveDir;
    [HideInInspector]
    public Vector2 lastMovedVector;

    public Rigidbody2D rigBod;
    public PlayerScriptableObject playerData;
    //public float moveSpeed;

    private bool canDash = true;
    private bool isDashing;
    private float dashingPower = 24f;
    private float dashingTime = 0.2f;
    private float dashingCooldown = 1f;

    [SerializeField] private TrailRenderer tr;

    // Start is called before the first frame update
    void Start()
    {
        rigBod = GetComponent<Rigidbody2D>();
        lastMovedVector = new Vector2(1, 0f); // Default last moved direction
    }

    // Update is called once per frame
    void Update()
    {
        if (isDashing)
        {
            return;
        }
        InputManagement();

        if (Input.GetKeyDown(KeyCode.LeftShift) && canDash)
        {
            StartCoroutine(Dash());
        }
    }

    private void FixedUpdate()
    {
        if (isDashing)
        {
            return;
        }

        Move();
    }

    void InputManagement()
    {
        float moveX = Input.GetAxisRaw("Horizontal");
        float moveY = Input.GetAxisRaw("Vertical");

        moveDir = new Vector2(moveX, moveY).normalized;

        if (moveDir.x != 0)
        {
            lastHorizontalVector = moveDir.x;
            lastMovedVector = new Vector2(lastHorizontalVector, 0f); // Last move in x direction
        }

        if (moveDir.y != 0)
        {
            lastVerticalVector = moveDir.y;
            lastMovedVector = new Vector2(0f, lastVerticalVector); // Last move in y direction
        }

        if (moveDir.x != 0 && moveDir.y != 0)
        {
            lastMovedVector = new Vector2(lastHorizontalVector, lastVerticalVector); // While moving in both directions
        }
    }

    void Move()
    {
        rigBod.velocity = new Vector2(moveDir.x * playerData.MoveSpeed, moveDir.y * playerData.MoveSpeed);
    }

    private IEnumerator Dash()
    {
        canDash = false;
        isDashing = true;
        Vector2 dashDirection = lastMovedVector.normalized;
        rigBod.velocity = dashDirection * dashingPower;
        tr.emitting = true;
        yield return new WaitForSeconds(dashingTime);
        tr.emitting = false;
        isDashing = false;
        rigBod.velocity = Vector2.zero;
        yield return new WaitForSeconds(dashingCooldown);
        canDash = true;
    }
}
