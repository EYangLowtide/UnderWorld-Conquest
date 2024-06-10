using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//projectile scripts if un use(place in prefab weapon that will be projectile)

public class PorjectileWeaponBehavior : MonoBehaviour
{
    protected Vector3 direction;
    public float destroyAfterSeconds;


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
}
