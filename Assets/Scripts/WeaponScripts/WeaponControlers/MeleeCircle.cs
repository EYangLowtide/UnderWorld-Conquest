using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeCircle : WeaponControler
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
        GameObject spawnedMelee = Instantiate(weaponData.Prefab);
        spawnedMelee.transform.position = transform.position; //assign same pos as this obj which is parented to player
        spawnedMelee.transform.parent = transform; //spawn below player
    }
}
