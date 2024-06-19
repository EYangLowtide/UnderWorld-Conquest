using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class EnemyStats : MonoBehaviour
{
    public EnemyScriptableObject enemyData;

    //Current stats
    [HideInInspector]
    public float currentMoveSpeed;
    [HideInInspector]
    public float currentHealth;
    [HideInInspector]
    public float currentDamage;

    public float despawnDistance = 20f;


    Transform player;
    public float dazedTime;
    public float startdDazingTime;
    //int currentEnemyHealth;

    private Animator anim;
    public GameObject bloodEffect;

    void Awake()
    {
        anim = GetComponent<Animator>();
        //Assign the vaiables
        currentMoveSpeed = enemyData.MoveSpeed;
        currentHealth = enemyData.MaxHealth;
        currentDamage = enemyData.Damage;
        //currentEnemyHealth = health;
        player = FindObjectOfType<PlayerMovement>().transform;
        anim.SetBool("isRunning", true);
    }
    void Start()
    {
        player = FindObjectOfType<PlayerStats>().transform;
    }

    void Update()
    {
        if (dazedTime <= 0)
        {
            currentMoveSpeed = enemyData.MoveSpeed;
        }
        else
        {
            currentMoveSpeed = 0;
            dazedTime -= Time.deltaTime;
        }

        if (currentHealth <= 0)
        {
            Destroy(gameObject);
        }
        player = FindObjectOfType<PlayerMovement>().transform;

        if (Vector2.Distance(transform.position, player.position) > despawnDistance)
        {
            ReturnEnemy();
        }

    }

    public void TakeDamage(float damage)
    {
        dazedTime = startdDazingTime;
        anim.SetTrigger("EnemyHurt");
        //play hurt sound
        //Instantiate(bloodEffect, transform.position, Quaternion.identity);
        //currentEnemyHealth -= damage;
        currentHealth -= damage;
        Debug.Log("EMOTIONAL DAMAGE!!! TAKEN");
        transform.position = Vector2.MoveTowards(transform.position, player.transform.position, enemyData.MoveSpeed * Time.deltaTime);    //Constantly move the enemy towards the player

    }

    public void Die()
    {
        Debug.Log("Enemy has been SLAIN!!!");

        anim.SetBool("EnemyIsDead", true);

        GetComponent<Collider2D>().enabled = false;
        this.enabled = false;
    }

    private void OnDestroy()
    {
        EnemySpawner es = FindObjectOfType<EnemySpawner>();
        es.OnEnemyKill();
    }

    void ReturnEnemy()
    {
        EnemySpawner es = FindObjectOfType<EnemySpawner>();
        transform.position = player.position + es.relativeSpawnPoints[UnityEngine.Random.Range(0, es.relativeSpawnPoints.Count)].position;
    }
}