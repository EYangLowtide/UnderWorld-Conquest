using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCollector : MonoBehaviour
{
    private PlayerStats player;
    private CircleCollider2D playerCollector;
    public float pullSpeed;

    private List<Rigidbody2D> itemsToPull = new List<Rigidbody2D>();

    void Start()
    {
        player = FindObjectOfType<PlayerStats>();
        playerCollector = GetComponent<CircleCollider2D>();
    }

    void Update()
    {
        UpdateCollectorRadius();
        PullItemsTowardsPlayer();
    }

    private void UpdateCollectorRadius()
    {
        playerCollector.radius = player.CurrentMagnet;
    }

    private void PullItemsTowardsPlayer()
    {
        foreach (var rb in itemsToPull)
        {
            if (rb != null)
            {
                Vector2 forceDirection = (transform.position - rb.transform.position).normalized;
                rb.AddForce(pullSpeed * Time.deltaTime * forceDirection);
            }
        }
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.TryGetComponent(out ICollectable collectable))
        {
            AddItemToPullList(col);
        }
    }

    private void AddItemToPullList(Collider2D col)
    {
        Rigidbody2D rb = col.GetComponent<Rigidbody2D>();
        if (rb != null && !itemsToPull.Contains(rb))
        {
            itemsToPull.Add(rb);
        }
    }
}
