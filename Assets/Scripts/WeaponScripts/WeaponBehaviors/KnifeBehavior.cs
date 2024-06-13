using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KnifeBehavior : PorjectileWeaponBehavior
{
    //KnifeController kc;


    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        //kc = FindObjectOfType<KnifeController>();
    }

    // Update is called once per frame
    void Update()
    {
        transform.position += direction * weaponData.Speed * Time.deltaTime; //move of knife
    }
}
