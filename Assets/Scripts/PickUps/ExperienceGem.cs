using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExperienceGem : PickUps, ICollectable
{
    public int experienceGranted;
    Rigidbody2D rb;

    bool hasTarget;
    Vector3 targetPos;
    float movSpd = 5f;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }
    public void Collect()
    {
        Debug.Log("Item Collected");
        PlayerStats player = FindObjectOfType<PlayerStats>();
        player.IncreaseExperience(experienceGranted);
        //Destroy(gameObject);
    }

    private void FixedUpdate()
    {
        if (hasTarget)
        {
            Vector2 targetDir = (targetPos - transform.position).normalized;
            rb.velocity = new Vector2(targetPos.x, targetPos.y) * movSpd;
        }
    }

    public void SetTarget(Vector3 position)
    {
        targetPos = position;
        hasTarget = true;
    }
}
