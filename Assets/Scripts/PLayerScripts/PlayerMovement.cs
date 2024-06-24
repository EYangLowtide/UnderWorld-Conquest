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
    //public PlayerScriptableObject playerData;
    //public float moveSpeed;
    PlayerStats player;

    public bool canDash = true;
    public bool isDashing;
    public bool isCollidingWithEnvironment = false;
    public float dashingPower = 24f;
    public float dashingTime = 0.2f;
    public float dashingCooldown = 1f;

    [Header("I-Frames")]
    public float invincibliltyDuration;
    public float invincibilityTimer;
    public bool isInvincable;

    [SerializeField] private TrailRenderer tr;

    private Vector2 collisionNormal;

    // Start is called before the first frame update
    void Start()
    {
        player = GetComponent<PlayerStats>();
        rigBod = GetComponent<Rigidbody2D>();
        lastMovedVector = new Vector2(1, 0f); // Default last moved direction
        rigBod.constraints = RigidbodyConstraints2D.FreezeRotation; // Prevent rotation due to physics interactions
    }

    // Update is called once per frame
    void Update()
    {
        if (GameManager.instance.isGameOver)
        {
            return;
        }

        if (isDashing)
        {
            invincibilityTimer = invincibliltyDuration;
            isInvincable = true;
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

        if (isCollidingWithEnvironment)
        {
            AdjustMoveDirection();
        }
        else
        {
            Move();
        }
    }

    void InputManagement()
    {
        if(GameManager.instance.isGameOver)
        {
            return;
        }
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
        if (GameManager.instance.isGameOver)
        {
            return;
        }

        if (moveDir != Vector2.zero)
        {
            rigBod.velocity = new Vector2(moveDir.x * player.CurrentMoveSpeed, moveDir.y * player.CurrentMoveSpeed); //was playerData.MoveSpeed
        }
        else
        {
            rigBod.velocity = Vector2.zero;
        }
    }

    private void AdjustMoveDirection()
    {
        Vector2 adjustedMoveDir = moveDir;

        // Prevent movement into the collision normal
        if (Vector2.Dot(moveDir, collisionNormal) < 0)
        {
            if (Mathf.Abs(collisionNormal.x) > Mathf.Abs(collisionNormal.y))
            {
                adjustedMoveDir.x = 0;
            }
            else
            {
                adjustedMoveDir.y = 0;
            }
        }

        rigBod.velocity = adjustedMoveDir * player.CurrentMoveSpeed; //Time.deltaTime; and was playerData
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
        rigBod.velocity = Vector2.zero; // Stop movement after dash
        yield return new WaitForSeconds(dashingCooldown);
        canDash = true;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Enviroment")) //erase the 2nd n in environment
        {
            isCollidingWithEnvironment = true;
            collisionNormal = (transform.position - collision.transform.position).normalized;//Time.deltaTime;
            rigBod.velocity = Vector2.zero;
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Enviroment"))
        {
            isCollidingWithEnvironment = true;
            collisionNormal = (transform.position - collision.transform.position).normalized;//Time.deltaTime;
            rigBod.velocity = Vector2.zero;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Enviroment"))
        {
            isCollidingWithEnvironment = false;
        }
    }
}
