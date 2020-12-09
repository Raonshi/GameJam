﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Key : MonoBehaviour
{


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public void OnTriggerEnter2D(Collider2D col)
    {
        if(col.CompareTag("Player"))
        {
            Player.instance.hasKey = true;
            SoundManager.Singleton.PlaySound(Resources.Load<AudioClip>("Sounds/Effect_Get_Key"));
            Destroy(gameObject);
        }
    }
}