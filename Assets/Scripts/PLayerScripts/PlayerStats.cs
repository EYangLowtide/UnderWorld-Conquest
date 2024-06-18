using System.Collections;
using System.Collections.Generic;
using UnityEditor.U2D.Animation;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    public PlayerScriptableObject playerData;

    float currentHealth;
    float currentMoveSpeed;
    float currentRecovery;
    float currentStrength;
    float currentProjectileSpeed;
    float currentAttackSpeed;
    float currentDashRange;

    //experience and level up
    [Header("Experience/Level")]
    public int experience = 0;
    public int level = 1;
    public int experienceCap = 100;

    //class for define level range and xp cap
    [System.Serializable]
    public class LevelRange
    {
        public int startLevel;
        public int endLevel;
        public int experienceCapIncrease;
    }

    //I-Frames
    [Header("I-Frames")]
    public float invincibliltyDuration;
    float invincibilityTimer;
    bool isInvincable;

    public List<LevelRange> levelsRanges;

    void Awake()
    {
        currentAttackSpeed = playerData.AttackSpeed;
        currentRecovery = playerData.Recovery;
        currentStrength = playerData.Strength;
        currentMoveSpeed = playerData.MoveSpeed;
        currentProjectileSpeed = playerData.ProjectileSpeed;
        currentDashRange = playerData.DashRange;
        currentHealth = playerData.MaxHealth;

    }

    void Start()
    {
        //init the xp cap as first increase
        experienceCap = levelsRanges[0].experienceCapIncrease;
    }

    // Start is called before the first frame update
    void Update()
    {
        if(invincibilityTimer > 0)
        {
            invincibilityTimer -= Time.deltaTime;
        }
        //is invincable time is 0 set invincablility to false
        else if (isInvincable)
        {
            isInvincable = false;
        }
    }

    public void IncreaseExperience(int amount)
    {
        experience += amount;

        LevelUpChecker();
    }

    void LevelUpChecker()
    {
        if(experience >= experienceCap)// increase lvl 
        {
            //lvl plyr up and deduct exp
            level++;
            experience -= experienceCap;
            //experience += experienceCapIncrease;
            int experienceCapIncrease = 0;
            foreach(LevelRange range in levelsRanges)
            {
                if (level >= range.startLevel && level <= range.endLevel)
                {
                    experienceCapIncrease = range.experienceCapIncrease;
                    break;
                }
            }
            experienceCap += experienceCapIncrease;
        }

    }

    public void TakeDamage(float dmg)
    {
       //if player is not invincable take dmg then become invincable
        if (!isInvincable)
        {
            currentHealth -= dmg;

            invincibilityTimer = invincibliltyDuration;
            isInvincable = true;

            if (currentHealth <= 0)
            {
                Kill();
            }
        }
    }

    public void Kill()
    {
        //Destroy(gameObject);
        Debug.Log("PLAYER HAS BEEN SLAIN!!!!!");
    }

    public void RestoreHealth(float amount)
    {
        if (currentHealth < playerData.MaxHealth)
        {
            currentHealth += amount;

            if (currentHealth > playerData.MaxHealth)
            {
                currentHealth = playerData.MaxHealth;
            }
        }

    }
}
