using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Magnet : MonoBehaviour
{
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.TryGetComponent<ExperienceGem>(out ExperienceGem experienceGem))
        {
            experienceGem.SetTarget(transform.parent.position);
        }

        if (collision.gameObject.TryGetComponent<HealthPotion>(out HealthPotion healthPotion))
        {
            experienceGem.SetTarget(transform.parent.position);
        }
    }
}
