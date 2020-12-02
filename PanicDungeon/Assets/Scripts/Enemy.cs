using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Enemy : MonoBehaviour
{
    public EModule emodule = new EModule();
    EnemyFOV enemyFOV;
    [SerializeField]
    Transform Tr;
    [SerializeField]
    Transform playerTr;

    public Transform target;

    public float WalkSpeed;
    public float RunSpeed;

    public float time = 1.0f;
    public float rotation;

    void Start()
    {
        rotation = 0;
        
        emodule.type = EModule.Type.Enemy1;
        emodule.InitEnemy();
        WalkSpeed = emodule.WalkSpeed;
        RunSpeed = emodule.RunSpeed;
        Tr = GetComponent<Transform>();
        playerTr = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        enemyFOV = GetComponent<EnemyFOV>();
    }

    void FixedUpdate()
    {
        time -= Time.deltaTime;

        StartCoroutine(CheckEnemyState());
        StartCoroutine(ActionEnemy());
    }

    IEnumerator CheckEnemyState()
    {
        yield return new WaitForSeconds(0.1f);
        if (enemyFOV.IsViewPlayer())
            emodule.state = EModule.EnemyState.trace;
        else
            emodule.state = EModule.EnemyState.patrol;
    }

    IEnumerator ActionEnemy()
    {
        switch (emodule.state)
        {
            case EModule.EnemyState.trace:
                //플레이어를 쫒는 코드 추가
                UpdateDirection();
                transform.position = Vector2.MoveTowards(transform.position, target.position, RunSpeed * Time.deltaTime);
                Debug.Log("추격");
                break;
            case EModule.EnemyState.patrol:

                Patrol();

                Debug.Log("순찰");


                //yield return new WaitForSeconds(0.2f);
                yield return null;
                break;
        }

        void UpdateDirection()
        {
            Vector2 dist = transform.position - target.position;


            //좌우
            if (Mathf.Abs(dist.x) > Mathf.Abs(dist.y))
            {
                //왼쪽 바라봄
                if (dist.x > 0)
                {
                    transform.rotation = Quaternion.Euler(0, 0, 0);
                }
                //오른쪽 바라봄
                else if (dist.x < 0)
                {
                    transform.rotation = Quaternion.Euler(0, 180, 0);
                }
            }
            //상하
            else if (Mathf.Abs(dist.x) < Mathf.Abs(dist.y))
            {
                //아래쪽 바라봄
                if (dist.y > 0)
                {
                    transform.rotation = Quaternion.Euler(0, 0, 90);
                }
                //위쪽 바라봄
                else if (dist.y < 0)
                {
                    transform.rotation = Quaternion.Euler(0, 0, 270);
                }
            }
        }
    }


    public  void Patrol()
    {
        if(time <= 0)
        {
            Debug.Log(rotation);
            rotation +=  90;
            transform.rotation = Quaternion.Euler(0, 0, rotation);
            time = 1.0f;
        }

        transform.Translate(Vector2.left * WalkSpeed * Time.deltaTime);
        
    }


    void OnCollisionEnter2D(Collision2D other)
    {
        if (other.transform.CompareTag("Player"))
        {
            Destroy(other.gameObject);
            Debug.Log("GameOver");
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }

}
