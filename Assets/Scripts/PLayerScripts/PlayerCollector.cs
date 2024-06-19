using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCollector : MonoBehaviour
{
    PlayerStats player;
    CircleCollider2D playerCollector;
    public float pullSpeed;

    private List<Rigidbody2D> itemsToPull = new List<Rigidbody2D>();

    void Start()
    {
       
        player = FindAnyObjectByType<PlayerStats>();
        playerCollector = GetComponent<CircleCollider2D>();
        playerCollector.isTrigger = true; // Ensure the collider is set as a trigger
          
        

    }

    void Update()
    {
        playerCollector.radius = player.currentMagnet;

        // Apply a gradual force to each item towards the player
        foreach (var rb in itemsToPull)
        {
            if (rb != null)
            {
                Vector2 forceDirection = (transform.position - rb.transform.position).normalized;
                rb.AddForce(forceDirection * pullSpeed * Time.deltaTime);
            }
        }


    }

    void OnTriggerEnter2D(Collider2D col)
    {
        
        if (col.gameObject.TryGetComponent(out ICollectable collectable))
        {
            // Adds items to the list for gradual pulling
            Rigidbody2D rb = col.GetComponent<Rigidbody2D>();
            if (rb != null && !itemsToPull.Contains(rb))
            {
                itemsToPull.Add(rb);
            }
        }
        /*//use stay2d for the one under
        ICollectable collectable = col.GetComponent<ICollectable>();
        if (collectable != null)
        {
            collectable.Collect();
        }
        */
    }
}
