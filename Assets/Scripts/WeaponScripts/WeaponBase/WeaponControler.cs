using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponControler : MonoBehaviour
{
    [Header("Weapon Stats")]
    public GameObject prefab;
    public float damage;
    public float speed;
    public float coolDownDuration;
    float currentCoolDown;
    public int pierce;

    protected PlayerMovement pm;


    //base script for weapon controller

    // Start is called before the first frame update
    protected virtual void Start()
    {
        pm = FindObjectOfType<PlayerMovement>();
        currentCoolDown = coolDownDuration;//cooldown at start to be the duration
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        currentCoolDown -= Time.deltaTime;
        if (currentCoolDown <= 0f) //once CD is 0 attack
        {
            Attack();
        }
    }

    protected virtual void Attack()
    {
        currentCoolDown = coolDownDuration;
    }
}
