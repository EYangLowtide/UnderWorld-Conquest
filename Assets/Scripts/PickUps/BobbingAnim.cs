using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BobbingAnim : MonoBehaviour
{
    public float frequency;
    public float magnitude;
    public Vector3 direction;
    Vector3 initalPostion;
    // Start is called before the first frame update
    void Start()
    {
        initalPostion = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = initalPostion + direction * Mathf.Sin(Time.time * frequency) * magnitude;
    }
}
