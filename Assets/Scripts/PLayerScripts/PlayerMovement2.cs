using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SHG.AnimatorCoder;

public class PlayerMovement2 : AnimatorCoder
{
    public static PlayerMovement2 instance;
    private float movementX = 0;
    private float movementY = 0;
    private float movementSpeed = 5;
    private Rigidbody2D rb;
    private SpriteRenderer sprite;

    private void Awake()
    {
        instance = this;
    }
    // Start is called before the first frame update
    void Start()
    {
        Initialize();
        rb = GetComponent<Rigidbody2D>();
        sprite = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        movementX = Input.GetAxisRaw("Horizontal");
        movementY = Input.GetAxisRaw("Vertical");
        CheckDie();
        CheckHit();
        CheckAttack();
        DefaultAnimation(0);

        void CheckDie()
        {
            if(!Input.GetKeyDown(KeyCode.LeftAlt)) return;

            SetLocked(false, 0);
            Play(new(Animations.DIE, true, new()));
            //Destroy(gameObject);
        }

        void CheckHit()
        {
            if(!Input.GetKeyDown(KeyCode.LeftControl)) return;

            SetLocked(false, 0);
            Play(new(Animations.HIT, true, new()));
        }

        void CheckAttack()
        {
            if (!Input.GetKeyDown(KeyCode.LeftShift)) return;

            Play(new(Animations.ATTACK1, true, new()));
        }
    }

    private void FixedUpdate()
    {
        rb.velocity = new(movementSpeed * movementX, movementSpeed * movementY);
    }

    public override void DefaultAnimation(int layer)
    {
        if(movementX == 0 && movementY == 0) Play(new(Animations.IDLE));
        else Play(new(Animations.WALK));
        if (GetCurrentAnimation(0) != Animations.ATTACK1 && movementX != 0) sprite.flipX = movementX < 0;
    }
}
