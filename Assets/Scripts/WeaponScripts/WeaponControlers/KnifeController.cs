using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KnifeController : WeaponControler
{


    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
    }

    protected override void Attack()
    {
        base.Attack();
        GameObject spawnedKnife = Instantiate(prefab);
        spawnedKnife.transform.position = transform.position; // knife start at the same pos as player 
        spawnedKnife.GetComponent<KnifeBehavior>().DirectionChecker(pm.lastMovedVector); //ref and set dir
    }
}
