using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthPotion : WeaponControler
{
    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Attack();
        GameObject spawnedHealth = Instantiate(weaponData.Prefab);
        spawnedHealth.transform.position = transform.position; //assign same pos as this obj which is parented to player
        spawnedHealth.transform.parent = transform; //spawn below player
    }
}
