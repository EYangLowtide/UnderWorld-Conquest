using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    Transform player;
    public int health;
    public float moveSpeed;
    public float dazedTime;
    public float startdDazingTime;

    private Animator anim;
    public GameObject bloodEffect;

    // Start is called before the first frame update
    void Start()
    {
        player = FindObjectOfType<PlayerMovement>().transform;
        anim = GetComponent<Animator>();
        anim.SetBool("isRunning", true);
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = Vector2.MoveTowards(transform.position, player.transform.position, moveSpeed * Time.deltaTime);
        if (dazedTime <= 0)
        {
            moveSpeed = 2;
        }
        else
        {
            moveSpeed = 0;
            dazedTime -= Time.deltaTime;
        }

        if (health <= 0)
        {
            Destroy(gameObject);
        }
        transform.Translate(Vector2.left *  moveSpeed * Time.deltaTime);
    }

    public void TakeDamage(int damage)
    {
        dazedTime = startdDazingTime; 
        //play hurt sound
        Instantiate(bloodEffect, transform.position, Quaternion.identity);
        health -= damage;
        Debug.Log("EMOTIONAL DAMAGE!!! TAKEN");
    }
}
