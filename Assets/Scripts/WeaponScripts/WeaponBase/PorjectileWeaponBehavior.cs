using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//projectile scripts if un use(place in prefab weapon that will be projectile)

public class PorjectileWeaponBehavior : MonoBehaviour
{
    public WeaponScriptableObject weaponData;

    protected Vector3 direction;
    public float destroyAfterSeconds;

    //current stats
    protected float currentDamage;
    protected float currentSpeed;
    protected float currentCoolDownduration;
    protected int currentPierce;

    void Awake()
    {
        currentDamage = weaponData.Damage;
        currentSpeed = weaponData.Speed;
        currentCoolDownduration = weaponData.CooldownDuration;
        currentPierce = weaponData.Pierce;
    }

    public float GetCurrentDamage()
    {
        return currentDamage *= FindObjectOfType<PlayerStats>().CurrentStrength;
    }

    // Start is called before the first frame update
    protected virtual void Start()
    {
        Destroy(gameObject, destroyAfterSeconds);
    }

    public void DirectionChecker(Vector3 dir)
    {
        direction = dir;
        float dirX = direction.x;
        float dirY = direction.y;

        Vector3 scale = transform.localScale;
        Vector3 rotation = transform.rotation.eulerAngles;

        if(dirX < 0 && dirY == 0)//left good
        {
            scale.x = scale.x * -1;
            scale.y = scale.y * -1;
        }
        else if (dirX == 0 && dirY < 0)//down 
        {
            scale.y = scale.y * -1;
            rotation.z = 45f;
        }
        else if (dirX == 0 && dirY > 0)//up
        {
            scale.x = scale.x * -1;
            rotation.z = 45f;
        }
        else if (dirX > 0 && dirY > 0)//right up
        {
            rotation.z = -90f;
        }
        else if (dirX > 0 && dirY < 0) //right down
        {
            rotation.z = -180f;
        }
        else if (dirX < 0 && dirY > 0) //left up good
        {
            scale.x = scale.x * -1;
            scale.y = scale.y * -1;
            rotation.z = 180f;
        }
        else if (dirX < 0 && dirY < 0) //left down
        {
            scale.x = scale.x * -1;
            scale.y = scale.y * -1;
            rotation.z = -90f;
        }

        transform.localScale = scale;
        transform.rotation = Quaternion.Euler(rotation); 
    }

    protected virtual void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("Enemy"))
        {
            EnemyStats enemy = col.GetComponent<EnemyStats>();
            enemy.TakeDamage(GetCurrentDamage()); //use current dmg instead of weaponData.damage for future ref
            ReducePierce();
        }
        else if (col.CompareTag("Props"))
        {
            if(col.gameObject.TryGetComponent(out BreakableProps breakable))
            {
                breakable.TakeDamage(GetCurrentDamage());
                ReducePierce();
            }
        }
    }

    void ReducePierce() //destroy throwable object when zero
    {
        currentPierce--;
        if (currentPierce <= 0)
        {
            Destroy(gameObject);
        }
    }
}
