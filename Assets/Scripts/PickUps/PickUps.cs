using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUps : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        //if object gets close to player destory objecct
        if (collision.CompareTag("Player"))
        {
            Destroy(gameObject);
        }
    }
}
