using System.Collections;
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
            transform.rotation = Quaternion.Euler(0,0,90);
            transform.Translate(Vector2.right * moveSpeed * Time.deltaTime);
        }
        else if(Input.GetKey(KeyCode.A))
        {
            transform.rotation = Quaternion.Euler(0,180,0);
            transform.Translate(Vector2.right * moveSpeed * Time.deltaTime);
        }
        else if(Input.GetKey(KeyCode.S))
        {
            transform.rotation = Quaternion.Euler(0,0,270);
            transform.Translate(Vector2.right * moveSpeed * Time.deltaTime);
        }
        else if(Input.GetKey(KeyCode.D))
        {
            transform.rotation = Quaternion.Euler(0,0,0);
            transform.Translate(Vector2.right * moveSpeed * Time.deltaTime);
        }
    }
}
