using System.Collections;
using System.Collections.Generic;
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


    public void AddWeapon(int slotIndex,  WeaponControler weapon) //add weapon to a slot
    {
        weaponSlots[slotIndex] = weapon;
        weaponLevels[slotIndex] = weapon.weaponData.Level;
        weaponUISlots[slotIndex].enabled = true;
        weaponUISlots[slotIndex].sprite = weapon.weaponData.Icon;
    }

    public void AddPassiveItem(int slotIndex, PassiveItems passiveItem) //add passive item to a slot
    {
        passiveItemSlots[slotIndex] = passiveItem;
        passiveItemLevels[slotIndex] = passiveItem.passiveItemData.Level;
        passiveItemUISlots[slotIndex].enabled |= true;
        passiveItemUISlots[slotIndex].sprite = passiveItem.passiveItemData.Icon;

    }

    public void LevelUpWeapon(int slotIndex)
    {
        if (weaponSlots.Count > slotIndex)
        {
            WeaponControler weapon = weaponSlots[slotIndex];
            if (!weapon.weaponData.NextLevelPrefab) //check for next level prefab
            {
                Debug.LogError("NO NEXT LEVEL FOR " + weapon.name);
                return;
            }
            GameObject upgradedWeapon = Instantiate(weapon.weaponData.NextLevelPrefab, transform.position, Quaternion.identity);
            upgradedWeapon.transform.SetParent(transform); //set weapon to be chile of palyer
            AddWeapon(slotIndex, upgradedWeapon.GetComponent<WeaponControler>());
            Destroy(weapon.gameObject);
            weaponLevels[slotIndex] = upgradedWeapon.GetComponent <WeaponControler>().weaponData.Level; //check weapon lvl
        }
    }

    public void LevelUpPassiveItem(int slotIndex)
    {
        if (passiveItemSlots.Count > slotIndex)
        {
            PassiveItems passiveItem = passiveItemSlots[slotIndex];
            if (!passiveItem.passiveItemData.NextLevelPrefab) //check for next level prefab
            {
                Debug.LogError("NO NEXT LEVEL FOR " + passiveItem.name);
                return;
            }
            GameObject upgradedPassiveItem = Instantiate(passiveItem.passiveItemData.NextLevelPrefab, transform.position, Quaternion.identity);
            upgradedPassiveItem.transform.SetParent(transform); //set weapon to be chile of palyer
            AddPassiveItem(slotIndex, upgradedPassiveItem.GetComponent<PassiveItems>());
            Destroy(passiveItem.gameObject);
            passiveItemLevels[slotIndex] = upgradedPassiveItem.GetComponent<PassiveItems>().passiveItemData.Level; //check passive item lvl
        }
    }
}
