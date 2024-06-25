using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthPotion : PickUps, ICollectable
{
    public int healthToRestore;
    Rigidbody2D rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }
    public override void Collect()
    {
        if (hasBeenCollected)
        {
            return;
        }
        else
        {
            base.Collect();
        }
        PlayerStats player = FindObjectOfType<PlayerStats>();
        player.RestoreHealth(healthToRestore);
        //Destroy(gameObject);
    }
}
