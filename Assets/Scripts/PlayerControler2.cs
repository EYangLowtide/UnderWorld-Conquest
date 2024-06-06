using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControler2 : MonoBehaviour
{ 
    public int movespeed;
    public Rigidbody2D characterRigBod;

    public float hInput;
    public float vInput;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        hInput = Input.GetAxis("Horizontal");
        vInput = Input.GetAxis("Vertical");
    }

    private void FixedUpdate()
    {
        characterRigBod.velocity = new Vector2 (hInput * movespeed, vInput * movespeed);
    }
}
