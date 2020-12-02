using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Enemy : MonoBehaviour
{
    public Transform target;

    
    
    public float moveSpeed;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        UpdateDirection();
        
        transform.position = Vector2.MoveTowards(transform.position, target.position, moveSpeed * Time.deltaTime);
        
        
    }

    public void UpdateDirection()
    {
        Vector2 dist = transform.position - target.position;


        //좌우
        if (Mathf.Abs(dist.x) > Mathf.Abs(dist.y))
        {
            //왼쪽 바라봄
            if (dist.x > 0)
            {
                transform.rotation = Quaternion.Euler(0,0,0);
            }
            //오른쪽 바라봄
            else if (dist.x < 0)
            {
                transform.rotation = Quaternion.Euler(0,180,0);
            }
        }
        //상하
        else if (Mathf.Abs(dist.x) < Mathf.Abs(dist.y))
        {
            //아래쪽 바라봄
            if (dist.y > 0)
            {
                transform.rotation = Quaternion.Euler(0,0,90);
            }
            //위쪽 바라봄
            else if (dist.y < 0)
            {
                transform.rotation = Quaternion.Euler(0,0,270);
            }
        }
    }

    public void OnCollisionEnter2D(Collision2D other)
    {
        if (other.transform.CompareTag("Player"))
        {
            Destroy(other.gameObject);
            Debug.Log("GameOver");
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }
}
