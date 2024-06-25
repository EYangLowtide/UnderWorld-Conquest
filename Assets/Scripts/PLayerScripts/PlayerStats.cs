using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class PlayerStats : MonoBehaviour
{
    public PlayerScriptableObject playerData;

    private Animator anim;

    private float currentHealth;
    private float currentMoveSpeed;
    private float currentRecovery;
    private float currentStrength;
    private float currentGuts;
    private float currentProjectileSpeed;
    private float currentAttackSpeed;
    private float currentDashRange;
    private float currentMagnet;

    #region Current Stats Properties

    public float CurrentHealth
    {
        get { return currentHealth; }
        set
        {
            if (currentHealth != value)
            {
                currentHealth = value;
                UpdateUI(GameManager.instance?.currentHealthDisplay, "Health: ", currentHealth);
                UpdateHealthBar();
            }
        }
    }
    public float CurrentMoveSpeed
    {
        get { return currentMoveSpeed; }
        set
        {
            if (currentMoveSpeed != value)
            {
                currentMoveSpeed = value;
                UpdateUI(GameManager.instance?.currentMoveSpeedDisplay, "Move Speed: ", currentMoveSpeed);
            }
        }
    }
    public float CurrentRecovery
    {
        get { return currentRecovery; }
        set
        {
            if (currentRecovery != value)
            {
                currentRecovery = value;
                UpdateUI(GameManager.instance?.currentRecoveryDisplay, "Regeneration: ", currentRecovery);
            }
        }
    }

    public float CurrentStrength
    {
        get { return currentStrength; }
        set
        {
            if (currentStrength != value)
            {
                currentStrength = value;
                UpdateUI(GameManager.instance?.currentStrengthDisplay, "Strength: ", currentStrength);
            }
        }
    }
    public float CurrentGuts
    {
        get { return currentGuts; }
        set
        {
            if (currentGuts != value)
            {
                currentGuts = value;
                UpdateUI(GameManager.instance?.currentGutsDisplay, "Guts: ", currentGuts);
            }
        }
    }
    public float CurrentProjectileSpeed
    {
        get { return currentProjectileSpeed; }
        set
        {
            if (currentProjectileSpeed != value)
            {
                currentProjectileSpeed = value;
                UpdateUI(GameManager.instance?.currentProjectileSpeedDisplay, "Projectile Speed: ", currentProjectileSpeed);
            }
        }
    }
    public float CurrentDashRange
    {
        get { return currentDashRange; }
        set
        {
            if (currentDashRange != value)
            {
                currentDashRange = value;
                UpdateUI(GameManager.instance?.currentDashRangeDisplay, "Dash Range: ", currentDashRange);
            }
        }
    }
    public float CurrentMagnet
    {
        get { return currentMagnet; }
        set
        {
            if (currentMagnet != value)
            {
                currentMagnet = value;
                UpdateUI(GameManager.instance?.currentMagnetDisplay, "Magnet Range: ", currentMagnet);
            }
        }
    }
    public float CurrentAttackSpeed
    {
        get { return currentAttackSpeed; }
        set
        {
            if (currentAttackSpeed != value)
            {
                currentAttackSpeed = value;
                UpdateUI(GameManager.instance?.currentAttackSpeedDisplay, "Attack Speed: ", currentAttackSpeed);
            }
        }
    }

    #endregion

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

    private InventoryManager inventory;
    private int weaponIndex;
    private int passiveItemIndex;

    [Header("UI")]
    public Image healthBar;
    public Image xpBar;
    public TMP_Text levelText;

    public GameObject firstPassiveItemTest, secondPassiveItemTest, thirdPassiveItemTest;

    private void Awake()
    {
        InitializeStats();
        SpawnInitialItems();
    }

    private void Start()
    {
        experienceCap = levelsRanges[0].experienceCapIncrease;
        UpdateAllUI();
        GameManager.instance.AssignChosenPlayerUI(playerData);
        UpdateHealthBar();
        UpdateXpBar();
        UpdateLevelText();
    }

    private void Update()
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
        UpdateHealthBar();
    }

    public void InitializeStats()
    {
        playerData = CharacterSelecter.GetData();
        CharacterSelecter.instance.DestroySingleton();

        inventory = GetComponent<InventoryManager>();
        anim = GetComponent<Animator>();

        CurrentAttackSpeed = playerData.AttackSpeed;
        CurrentRecovery = playerData.Recovery;
        CurrentStrength = playerData.Strength;
        CurrentGuts = playerData.Guts;
        CurrentMoveSpeed = playerData.MoveSpeed;
        CurrentProjectileSpeed = playerData.ProjectileSpeed;
        CurrentDashRange = playerData.DashRange;
        CurrentHealth = playerData.MaxHealth;
        CurrentMagnet = playerData.Magnet;
    }

    private void UpdateAllUI()
    {
        UpdateUI(GameManager.instance?.currentHealthDisplay, "Health: ", currentHealth);
        UpdateUI(GameManager.instance?.currentMoveSpeedDisplay, "Move Speed: ", currentMoveSpeed);
        UpdateUI(GameManager.instance?.currentRecoveryDisplay, "Regeneration: ", currentRecovery);
        UpdateUI(GameManager.instance?.currentStrengthDisplay, "Strength: ", currentStrength);
        UpdateUI(GameManager.instance?.currentGutsDisplay, "Guts: ", currentGuts);
        UpdateUI(GameManager.instance?.currentProjectileSpeedDisplay, "Projectile Speed: ", currentProjectileSpeed);
        UpdateUI(GameManager.instance?.currentDashRangeDisplay, "Dash Range: ", currentDashRange);
        UpdateUI(GameManager.instance?.currentMagnetDisplay, "Magnet Range: ", currentMagnet);
        UpdateUI(GameManager.instance?.currentAttackSpeedDisplay, "Attack Speed: ", currentAttackSpeed);
    }

    private void UpdateUI(TMP_Text textComponent, string label, float value)
    {
        if (textComponent != null)
        {
            textComponent.text = $"{label} {value}";
        }
    }

    public void IncreaseExperience(int amount)
    {
        experience += amount;
        LevelUpChecker();
        UpdateXpBar();
    }

    private void LevelUpChecker()
    {
        while (experience >= experienceCap)
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

            UpdateLevelText();

            GameManager.instance.StartLevelUp();
        }
    }

    void UpdateXpBar()
    {
        xpBar.fillAmount = (float)experience / experienceCap;
    }

    void UpdateLevelText()
    {
        levelText.text = "LV " + level.ToString();
    }

    public void TakeDamage(float dmg)
    {
        if (!isInvincible)
        {
            anim.SetTrigger("PlayerHurt");
            CurrentHealth -= dmg;

            invincibilityTimer = invincibilityDuration;
            isInvincible = true;

            if (CurrentHealth <= 0)
            {
                Kill();
            }

            UpdateHealthBar();
        }
    }

    void UpdateHealthBar()
    {
        healthBar.fillAmount = currentHealth / playerData.MaxHealth;
    }

    public void Kill()
    {
        if (!GameManager.instance.isGameOver)
        {
            anim.SetBool("PlayerIsDead", true);
            GameManager.instance.AssignLevelReachedUI(level);
            GameManager.instance.AssignChosenWeaponAndPassiveUI(inventory.weaponUISlots, inventory.passiveItemUISlots);
            GameManager.instance.GameOver();
            StartCoroutine(HandleDeath());
        }
    }

    private IEnumerator HandleDeath()
    {
        yield return new WaitForSeconds(anim.GetCurrentAnimatorStateInfo(0).length);

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

        Destroy(gameObject);
    }

    public void RestoreHealth(float amount)
    {
        if (CurrentHealth < playerData.MaxHealth)
        {
            CurrentHealth += amount;

            if (CurrentHealth > playerData.MaxHealth)
            {
                CurrentHealth = playerData.MaxHealth;
            }

            UpdateHealthBar(); // Ensure the health bar is updated
        }
    }

    public void RegenerateHealth()
    {
        if (CurrentHealth < playerData.MaxHealth)
        {
            CurrentHealth += CurrentRecovery * Time.deltaTime;

            if (CurrentHealth > playerData.MaxHealth)
            {
                CurrentHealth = playerData.MaxHealth;
            }
            UpdateHealthBar();
        }
    }

    private void SpawnInitialItems()
    {
        SpawnedWeapon(playerData.StartingWeapon);
        SpawnedPassiveItem(firstPassiveItemTest);
        SpawnedPassiveItem(secondPassiveItemTest);
        SpawnedPassiveItem(thirdPassiveItemTest);
    }

    public void SpawnedWeapon(GameObject weapon)
    {
        if (weaponIndex >= inventory.weaponSlots.Count - 1)
        {
            Debug.LogError("Inventory slots already full");
            return;
        }

        GameObject spawnedWeapon = Instantiate(weapon, transform.position, Quaternion.identity);
        spawnedWeapon.transform.SetParent(transform);
        inventory.AddWeapon(weaponIndex, spawnedWeapon.GetComponent<WeaponControler>());

        weaponIndex++;
    }

    public void SpawnedPassiveItem(GameObject passiveItem)
    {
        if (passiveItemIndex >= inventory.passiveItemSlots.Count - 1)
        {
            Debug.LogError("Inventory slots already full");
            return;
        }

        GameObject spawnedPassiveItem = Instantiate(passiveItem, transform.position, Quaternion.identity);
        spawnedPassiveItem.transform.SetParent(transform);
        inventory.AddPassiveItem(passiveItemIndex, spawnedPassiveItem.GetComponent<PassiveItems>());

        passiveItemIndex++;
    }

    public void ResetStats()
    {
        InitializeStats();
        UpdateAllUI();
    }
}
