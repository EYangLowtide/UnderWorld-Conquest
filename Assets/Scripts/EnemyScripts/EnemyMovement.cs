using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    public EnemyScriptableObject enemyData;
    public Vector2 moveDir;  // Add this line
    Transform player;

    void Start()
    {
        player = FindObjectOfType<PlayerMovement>().transform;
    }

    void Update()
    {
        Vector2 newPosition = Vector2.MoveTowards(transform.position, player.transform.position, enemyData.MoveSpeed * Time.deltaTime);
        moveDir = newPosition - (Vector2)transform.position;  // Update the moveDir
        transform.position = newPosition;  // Move the enemy towards the player

    }
}
