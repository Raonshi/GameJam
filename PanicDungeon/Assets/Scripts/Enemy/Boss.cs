﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Boss : MonoBehaviour
{
    public EModule emodule = new EModule();
    EnemyFOV enemyFOV;
    [SerializeField] Transform target;
    [SerializeField] Transform position;

    public float WalkSpeed;
    public float RunSpeed;
    public float time = 1.0f;
    public float rotation;
    public bool isKnow;

    void Start()
    {
        rotation = 0;
        emodule.type = EModule.Type.Enemy2;
        emodule.InitEnemy();
        WalkSpeed = emodule.WalkSpeed;
        RunSpeed = emodule.RunSpeed;
        target = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        enemyFOV = GetComponent<EnemyFOV>();
        isKnow = false;
        position = null;
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
        else if (!enemyFOV.IsViewPlayer() && isKnow)
            emodule.state = EModule.EnemyState.caution;
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
                transform.position = Vector3.MoveTowards(transform.position, target.position, RunSpeed * Time.deltaTime);
                isKnow = false;
                Debug.Log("추격");
                break;

            case EModule.EnemyState.patrol:
                Patrol();
                Debug.Log("순찰");
                yield return null;
                break;

            case EModule.EnemyState.caution:
                Debug.Log("주의");
                transform.position = Vector2.MoveTowards(transform.position, position.position, WalkSpeed * Time.deltaTime);
                float dist = Vector2.Distance(transform.position, position.position);
                if (dist <= 1f)
                {
                    UnEnableIsKnow();
                }
                break;
        }
    }

    void UpdateDirection()
    {
        Vector3 dist = transform.position - target.position;


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
                transform.rotation = Quaternion.Euler(0, 0, 180);
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

    public void Patrol()
    {
        int rand = Random.Range(-2, 3);
        if (time <= 0)
        {
            rotation += 90 * rand;
            transform.rotation = Quaternion.Euler(0, 0, rotation);
            time = 1.0f;
        }

        transform.Translate(Vector3.left * WalkSpeed * Time.deltaTime);

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

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Interact"))
        {
            EnableIsKnow();
            position = other.GetComponent<Transform>();
        }
    }

    void EnableIsKnow()
    {
        isKnow = true;
    }

    void UnEnableIsKnow()
    {
        isKnow = false;
    }
}