using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InventoryManager : MonoBehaviour
{
    public List<WeaponControler> weaponSlots = new List<WeaponControler>(8);
    public int[] weaponLevels = new int[8];
    public List<Image> weaponUISlots = new List<Image>(8);
    public List<PassiveItems> passiveItemSlots = new List<PassiveItems>(8);
    public int[] passiveItemLevels = new int[8];
    public List<Image> passiveItemUISlots = new List<Image>(8);

    [System.Serializable]
    public class WeaponUpgrade
    {
        public int weaponUpgradeIndex;
        public GameObject initialWeapon;
        public WeaponScriptableObject weaponData;
    }

    [System.Serializable]
    public class PassiveItemUpgrade
    {
        public int passiveUpgradeIndex;
        public GameObject initialPassiveItem;
        public PassiveItemScriptableObjects passiveData;
    }

    [System.Serializable]
    public class UpgradeUI
    {
        public TMP_Text upgradeNameDisplay;
        public TMP_Text upgradeDescriptionDisplay;
        public Image upgradeIcon;
        public Button upgradeButton;
    }

    public List<WeaponUpgrade> weaponUpgradeOptions = new List<WeaponUpgrade>();
    public List<PassiveItemUpgrade> passiveItemUpgradeOptions = new List<PassiveItemUpgrade>();
    public List<UpgradeUI> upgradeUIOptions = new List<UpgradeUI>();

    private PlayerStats player;

    void Start()
    {
        player = GetComponent<PlayerStats>();
    }

    public void AddWeapon(int slotIndex, WeaponControler weapon)
    {
        if (slotIndex < 0 || slotIndex >= weaponSlots.Count)
        {
            Debug.LogWarning("Invalid weapon slot index.");
            return;
        }

        weaponSlots[slotIndex] = weapon;
        weaponLevels[slotIndex] = weapon.weaponData.Level;
        UpdateWeaponUI(slotIndex, weapon.weaponData.Icon);

        if (GameManager.instance != null && GameManager.instance.choosingUpgrade)
        {
            GameManager.instance.EndLevelUp();
        }
    }

    public void AddPassiveItem(int slotIndex, PassiveItems passiveItem)
    {
        if (slotIndex < 0 || slotIndex >= passiveItemSlots.Count)
        {
            Debug.LogWarning("Invalid passive item slot index.");
            return;
        }

        passiveItemSlots[slotIndex] = passiveItem;
        passiveItemLevels[slotIndex] = passiveItem.passiveItemData.Level;
        UpdatePassiveItemUI(slotIndex, passiveItem.passiveItemData.Icon);

        if (GameManager.instance != null && GameManager.instance.choosingUpgrade)
        {
            GameManager.instance.EndLevelUp();
        }
    }

    public void LevelUpWeapon(int slotIndex, int upgradeIndex)
    {
        if (slotIndex < 0 || slotIndex >= weaponSlots.Count)
        {
            Debug.LogWarning("Invalid weapon slot index.");
            return;
        }

        WeaponControler weapon = weaponSlots[slotIndex];
        if (!weapon.weaponData.NextLevelPrefab)
        {
            Debug.LogError("No next level for " + weapon.name);
            return;
        }

        GameObject upgradedWeapon = Instantiate(weapon.weaponData.NextLevelPrefab, transform.position, Quaternion.identity);
        upgradedWeapon.transform.SetParent(transform);
        AddWeapon(slotIndex, upgradedWeapon.GetComponent<WeaponControler>());
        Destroy(weapon.gameObject);

        weaponUpgradeOptions[upgradeIndex].weaponData = upgradedWeapon.GetComponent<WeaponControler>().weaponData;
    }

    public void LevelUpPassiveItem(int slotIndex, int upgradeIndex)
    {
        if (slotIndex < 0 || slotIndex >= passiveItemSlots.Count)
        {
            Debug.LogWarning("Invalid passive item slot index.");
            return;
        }

        PassiveItems passiveItem = passiveItemSlots[slotIndex];
        if (!passiveItem.passiveItemData.NextLevelPrefab)
        {
            Debug.LogError("No next level for " + passiveItem.name);
            return;
        }

        GameObject upgradedPassiveItem = Instantiate(passiveItem.passiveItemData.NextLevelPrefab, transform.position, Quaternion.identity);
        upgradedPassiveItem.transform.SetParent(transform);
        AddPassiveItem(slotIndex, upgradedPassiveItem.GetComponent<PassiveItems>());
        Destroy(passiveItem.gameObject);

        passiveItemUpgradeOptions[upgradeIndex].passiveData = upgradedPassiveItem.GetComponent<PassiveItems>().passiveItemData;
    }

    private void ApplyUpgradeOptions()
    {

        foreach (var upgradeOption in upgradeUIOptions)
        {
            int upgradeType = Random.Range(1, 3);
            if (upgradeType == 1)
            {
                ApplyWeaponUpgrade(upgradeOption);
            }
            else if (upgradeType == 2)
            {
                ApplyPassiveItemUpgrade(upgradeOption);
            }
        }
    }

    private void ApplyWeaponUpgrade(UpgradeUI upgradeOption)
    {
        List<WeaponUpgrade> availableWeaponUpgrades = new List<WeaponUpgrade>(weaponUpgradeOptions);

        WeaponUpgrade chosenWeaponUpgrade = availableWeaponUpgrades[Random.Range(0, availableWeaponUpgrades.Count)];

        availableWeaponUpgrades.Remove(chosenWeaponUpgrade);

        if (chosenWeaponUpgrade != null)
        {
            bool newWeapon = true;
            for (int i = 0; i < weaponSlots.Count; ++i)
            {
                // Check if the weapon slot is occupied and matches the chosen upgrade
                if (weaponSlots[i] != null && weaponSlots[i].weaponData == chosenWeaponUpgrade.weaponData)
                {
                    // Check if the chosen weapon has a next level defined
                    if (!chosenWeaponUpgrade.weaponData.NextLevelPrefab)
                    {
                        break; // Exit the loop if no next level is defined
                    }

                    newWeapon = false;
                    // Set up the UI for leveling up the existing weapon
                    upgradeOption.upgradeButton.onClick.AddListener(() => LevelUpWeapon(i, chosenWeaponUpgrade.weaponUpgradeIndex));
                    upgradeOption.upgradeDescriptionDisplay.text = chosenWeaponUpgrade.weaponData.NextLevelPrefab.GetComponent<WeaponControler>().weaponData.Description;
                    upgradeOption.upgradeNameDisplay.text = chosenWeaponUpgrade.weaponData.NextLevelPrefab.GetComponent<WeaponControler>().weaponData.Name;
                    break;
                }
            }
            if (newWeapon)
            {
                // Set up the UI for spawning a new weapon
                upgradeOption.upgradeButton.onClick.AddListener(() => player.SpawnedWeapon(chosenWeaponUpgrade.initialWeapon));
                upgradeOption.upgradeDescriptionDisplay.text = chosenWeaponUpgrade.weaponData.Description;
                upgradeOption.upgradeNameDisplay.text = chosenWeaponUpgrade.weaponData.Name;
            }
            upgradeOption.upgradeIcon.sprite = chosenWeaponUpgrade.weaponData.Icon;
        }
    }

    private void ApplyPassiveItemUpgrade(UpgradeUI upgradeOption)
    {
        List<PassiveItemUpgrade> availablePassiveUpgrades = new List<PassiveItemUpgrade>(passiveItemUpgradeOptions);

        PassiveItemUpgrade chosenPassiveItemUpgrade = availablePassiveUpgrades[Random.Range(0, availablePassiveUpgrades.Count)];

        availablePassiveUpgrades.Remove(chosenPassiveItemUpgrade);

        if (chosenPassiveItemUpgrade != null)
        {
            bool newPassiveItem = true;
            for (int i = 0; i < passiveItemSlots.Count; ++i)
            {
                // Check if the passive item slot is occupied and matches the chosen upgrade
                if (passiveItemSlots[i] != null && passiveItemSlots[i].passiveItemData == chosenPassiveItemUpgrade.passiveData)
                {
                    // Check if the chosen passive item has a next level defined
                    if (!chosenPassiveItemUpgrade.passiveData.NextLevelPrefab)
                    {
                        break; // Exit the loop if no next level is defined
                    }

                    newPassiveItem = false;
                    // Set up the UI for leveling up the existing passive item
                    upgradeOption.upgradeButton.onClick.AddListener(() => LevelUpPassiveItem(i, chosenPassiveItemUpgrade.passiveUpgradeIndex));
                    upgradeOption.upgradeDescriptionDisplay.text = chosenPassiveItemUpgrade.passiveData.NextLevelPrefab.GetComponent<PassiveItems>().passiveItemData.Description;
                    upgradeOption.upgradeNameDisplay.text = chosenPassiveItemUpgrade.passiveData.NextLevelPrefab.GetComponent<PassiveItems>().passiveItemData.Name;
                    break;
                }
            }
            if (newPassiveItem)
            {
                // Set up the UI for spawning a new passive item
                upgradeOption.upgradeButton.onClick.AddListener(() => player.SpawnedPassiveItem(chosenPassiveItemUpgrade.initialPassiveItem));
                upgradeOption.upgradeDescriptionDisplay.text = chosenPassiveItemUpgrade.passiveData.Description;
                upgradeOption.upgradeNameDisplay.text = chosenPassiveItemUpgrade.passiveData.Name;
            }
            upgradeOption.upgradeIcon.sprite = chosenPassiveItemUpgrade.passiveData.Icon;
        }
    }


    private void UpdateWeaponUI(int slotIndex, Sprite icon)
    {
        if (slotIndex < 0 || slotIndex >= weaponUISlots.Count)
        {
            Debug.LogWarning("Invalid weapon UI slot index.");
            return;
        }

        weaponUISlots[slotIndex].enabled = true;
        weaponUISlots[slotIndex].sprite = icon;
    }

    private void UpdatePassiveItemUI(int slotIndex, Sprite icon)
    {
        if (slotIndex < 0 || slotIndex >= passiveItemUISlots.Count)
        {
            Debug.LogWarning("Invalid passive item UI slot index.");
            return;
        }

        passiveItemUISlots[slotIndex].enabled = true;
        passiveItemUISlots[slotIndex].sprite = icon;
    }

    public void RemoveAndApplyUpgrades()
    {
        RemoveUpgradeOptions();
        ApplyUpgradeOptions();
    }

    private void RemoveUpgradeOptions()
    {
        foreach (var upgradeOption in upgradeUIOptions)
        {
            upgradeOption.upgradeButton.onClick.RemoveAllListeners();
        }
    }
}
