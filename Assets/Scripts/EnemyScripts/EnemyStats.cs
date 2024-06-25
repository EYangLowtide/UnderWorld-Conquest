using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class EnemyStats : MonoBehaviour
{
    public EnemyScriptableObject enemyData;

    [HideInInspector]
    public float currentMoveSpeed;
    [HideInInspector]
    public float currentHealth;
    [HideInInspector]
    public float currentDamage;

    public float despawnDistance = 20f;
    public float dazedTime;
    public float startDazingTime;
    public GameObject bloodEffect;

    private Transform player;
    private Animator anim;
    private bool isDying = false;

    void Awake()
    {
        anim = GetComponent<Animator>();
        InitializeStats();
        player = FindPlayer();
    }

    void Start()
    {
        player = FindPlayer();
    }

    void Update()
    {
        if (isDying) return;

        HandleDazedTime();
        CheckHealth();
        CheckDespawnDistance();
    }

    private void InitializeStats()
    {
        currentMoveSpeed = enemyData.MoveSpeed;
        currentHealth = enemyData.MaxHealth;
        currentDamage = enemyData.Damage;
        anim.SetBool("isRunning", true);
    }

    private Transform FindPlayer()
    {
        PlayerStats playerStats = FindObjectOfType<PlayerStats>();
        if (playerStats != null)
        {
            return playerStats.transform;
        }
        return null;
    }

    private void HandleDazedTime()
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
    }

    private void CheckHealth()
    {
        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private void CheckDespawnDistance()
    {
        if (player == null) return;

        if (Vector2.Distance(transform.position, player.position) > despawnDistance)
        {
            ReturnEnemy();
        }
    }

    public void TakeDamage(float damage)
    {
        dazedTime = startDazingTime;
        anim.SetTrigger("EnemyHurt");
        currentHealth -= damage;
        Debug.Log("EMOTIONAL DAMAGE!!! TAKEN");

        if (player != null)
        {
            transform.position = Vector2.MoveTowards(transform.position, player.position, enemyData.MoveSpeed * Time.deltaTime);
        }
    }

    public void Die()
    {
        if (isDying) return;

        isDying = true;
        Debug.Log("Enemy has been SLAIN!!!");
        anim.SetBool("EnemyIsDead", true);
        GetComponent<Collider2D>().enabled = false;
        StartCoroutine(HandleDeath());
    }

    private IEnumerator HandleDeath()
    {
        // Wait for the death animation to play fully
        yield return new WaitForSeconds(anim.GetCurrentAnimatorStateInfo(0).length);

        // Optionally instantiate blood effect or other effects
        if (bloodEffect != null)
        {
            Instantiate(bloodEffect, transform.position, Quaternion.identity);
        }

        // Destroy the GameObject
        Destroy(gameObject);
    }

    private void OnDestroy()
    {
        EnemySpawner es = FindObjectOfType<EnemySpawner>();
        if (es != null)
        {
            es.OnEnemyKill();
        }
    }

    private void ReturnEnemy()
    {
        EnemySpawner es = FindObjectOfType<EnemySpawner>();
        if (es != null && player != null)
        {
            transform.position = player.position + es.relativeSpawnPoints[UnityEngine.Random.Range(0, es.relativeSpawnPoints.Count)].position;
        }
    }
}
