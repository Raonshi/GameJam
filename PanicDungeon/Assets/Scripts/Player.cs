﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public float moveSpeed;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Control();
    }

    public void Control()
    {
        if (Input.GetKey(KeyCode.W))
        {
            transform.Translate(Vector2.up * moveSpeed * Time.deltaTime);
        }
        else if(Input.GetKey(KeyCode.A))
        {
            transform.Translate(Vector2.left * moveSpeed * Time.deltaTime);
        }
        else if(Input.GetKey(KeyCode.S))
        {
            transform.Translate(Vector2.down * moveSpeed * Time.deltaTime);
        }
        else if(Input.GetKey(KeyCode.D))
        {
            transform.Translate(Vector2.right * moveSpeed * Time.deltaTime);
        }
    }
}
