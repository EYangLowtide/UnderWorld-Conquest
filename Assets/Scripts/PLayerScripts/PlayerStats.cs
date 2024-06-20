using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using UnityEditor.U2D.Animation;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    PlayerScriptableObject playerData;

    private Animator anim;

    [HideInInspector] public float currentHealth;
    [HideInInspector] public float currentMoveSpeed;
    [HideInInspector] public float currentRecovery;
    [HideInInspector] public float currentStrength;
    [HideInInspector] public float currentGuts;
    [HideInInspector] public float currentProjectileSpeed;
    [HideInInspector] public float currentAttackSpeed;
    [HideInInspector] public float currentDashRange;
    [HideInInspector] public float currentMagnet;


    //spawn weapons
    //public List<GameObject> spawnedWeapons;

    // Experience and level up
    [Header("Experience/Level")]
    public int experience = 0;
    public int level = 1;
    public int experienceCap = 100;

    [System.Serializable]
    public class LevelRange
    {
        public int startLevel;
        public int endLevel;
        public int experienceCapIncrease;
    }

    [Header("I-Frames")]
    public float invincibilityDuration;
    public float invincibilityTimer;
    public bool isInvincible;

    public List<LevelRange> levelsRanges;

    InventoryManager inventory;
    public int weaponIndex;
    public int passiveItemIndex;

    //public GameObject secondWeaponTest;
    public GameObject firstPassiveItemTest, secondPassiveItemTest, thirdPassiveItemTest;

    void Awake()
    {
        playerData = CharacterSelecter.GetData();
        CharacterSelecter.instance.DestroySingleton();

        inventory = GetComponent<InventoryManager>();

        anim = GetComponent<Animator>();
        currentAttackSpeed = playerData.AttackSpeed;
        currentRecovery = playerData.Recovery;
        currentStrength = playerData.Strength;
        currentGuts = playerData.Guts;
        currentMoveSpeed = playerData.MoveSpeed;
        currentProjectileSpeed = playerData.ProjectileSpeed;
        currentDashRange = playerData.DashRange;
        currentHealth = playerData.MaxHealth;
        currentMagnet = playerData.Magnet;

        SpawnedWeapon(playerData.StartingWeapon);
        //SpawnedWeapon(secondWeaponTest);
        SpawnedPassiveItem(firstPassiveItemTest);
        SpawnedPassiveItem(secondPassiveItemTest);
        SpawnedPassiveItem(thirdPassiveItemTest);

    }

    void Start()
    {
        // Init the XP cap as first increase
        experienceCap = levelsRanges[0].experienceCapIncrease;
    }

    void Update()
    {
        if (invincibilityTimer > 0)
        {
            invincibilityTimer -= Time.deltaTime;
        }
        else if (isInvincible)
        {
            isInvincible = false;
        }

        RegenerateHealth();
    }

    public void IncreaseExperience(int amount)
    {
        experience += amount;
        LevelUpChecker();
    }

    void LevelUpChecker()
    {
        while (experience >= experienceCap) // Handle multiple level-ups in one go
        {
            level++;
            experience -= experienceCap;
            int experienceCapIncrease = 0;
            foreach (LevelRange range in levelsRanges)
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
        // If player is not invincible, take damage then become invincible
        if (!isInvincible)
        {
            anim.SetTrigger("PlayerHurt");
            currentHealth -= dmg;

            invincibilityTimer = invincibilityDuration;
            isInvincible = true;

            if (currentHealth <= 0)
            {
                Kill();
            }
        }
    }

    public void Kill()
    {
        anim.SetBool("PlayerIsDead", true);
        StartCoroutine(HandleDeath());
    }

    private IEnumerator HandleDeath()
    {
        // Wait for the death animation to play fully
        yield return new WaitForSeconds(anim.GetCurrentAnimatorStateInfo(0).length);

        // Option 1: Destroy the GameObject
        // Destroy(gameObject);

        // Option 2: Fade out the GameObject
        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteRenderer != null)
        {
            float fadeDuration = 2f;
            float fadeSpeed = 1f / fadeDuration;
            Color color = spriteRenderer.color;

            for (float t = 0; t < 1f; t += Time.deltaTime * fadeSpeed)
            {
                color.a = Mathf.Lerp(1f, 0f, t);
                spriteRenderer.color = color;
                yield return null;
            }

            color.a = 0f;
            spriteRenderer.color = color;
        }

        // Destroy the GameObject after fading out
        Destroy(gameObject);
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

    public void RegenerateHealth()
    {
        if (currentHealth < playerData.MaxHealth)
        {
            currentHealth += currentRecovery * Time.deltaTime;

            // Make sure regen does not go over max health
            if (currentHealth > playerData.MaxHealth)
            {
                currentHealth = playerData.MaxHealth;
            }
        }
    }

    public void SpawnedWeapon(GameObject weapon)
    {
        //check is slots are full
        if (weaponIndex >= inventory.weaponSlots.Count - 1) //starts at 0 so make -1 
        {
            Debug.LogError("Inventory slots already full");
            return;
        }
        //starting  weapon spawn
        GameObject spawnedWeapon = Instantiate(weapon, transform.position, Quaternion.identity);
        spawnedWeapon.transform.SetParent(transform);
        //spawnedWeapons.Add(spawnedWeapon);
        inventory.AddWeapon(weaponIndex, spawnedWeapon.GetComponent<WeaponControler>()); //add weapon to inventory slot

        weaponIndex++;
    }

    public void SpawnedPassiveItem(GameObject passiveItem)
    {
        //check is slots are full
        if (weaponIndex >= inventory.passiveItemSlots.Count - 1) //starts at 0 so make -1 
        {
            Debug.LogError("Inventory slots already full");
            return;
        }
        //starting passive item spawn
        GameObject spawnedPassiveItem = Instantiate(passiveItem, transform.position, Quaternion.identity);
        spawnedPassiveItem.transform.SetParent(transform);
        inventory.AddPassiveItem(passiveItemIndex, spawnedPassiveItem.GetComponent<PassiveItems>()); //add weapon to inventory slot

        passiveItemIndex++;
    }
}
