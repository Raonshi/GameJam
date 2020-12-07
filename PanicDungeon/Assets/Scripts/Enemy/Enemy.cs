using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Enemy : MonoBehaviour
{
    public EModule emodule = new EModule();
    EnemyFOV enemyFOV;
    Player player;
    [SerializeField]
    Transform Tr;
    [SerializeField]
    Transform playerTr;
    [SerializeField]
    Transform position;
    public Game game;

    public Transform target;

    public float WalkSpeed;
    public float RunSpeed;
    public bool isKnow;
    public bool IsClear;

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
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();

        isKnow = false;
        position = null;
        game = Game.instance;
        IsClear = false;
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
        if (IsClear)
            emodule.state = EModule.EnemyState.Dead;
        else if (enemyFOV.IsViewPlayer())
            emodule.state = EModule.EnemyState.trace;
        else if(!enemyFOV.IsViewPlayer() && isKnow)
            emodule.state = EModule.EnemyState.caution;
        else
            emodule.state = EModule.EnemyState.patrol;
    }

    IEnumerator ActionEnemy()
    {
        switch (emodule.state)
        {
            case EModule.EnemyState.trace:
                UpdateDirection();
                transform.position = Vector3.MoveTowards(transform.position, target.position, RunSpeed * Time.deltaTime);
                isKnow = false;
                Debug.Log("추격");
                break;

            case EModule.EnemyState.patrol:
                //Patrol();
                Debug.Log("순찰");
                yield return null;
                break;

            case EModule.EnemyState.caution:
                Debug.Log("주의");
                transform.position = Vector2.MoveTowards(transform.position, position.position, WalkSpeed * Time.deltaTime);
                float dist = Vector2.Distance(transform.position, position.position);
                if(dist <= 1f)
                {
                    UnEnableIsKnow();
                }              
                break;

            case EModule.EnemyState.Dead:
                player.catchCount += 1;
                Destroy(gameObject);
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
        if(time <= 0)
        {
            rotation +=  90;
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

    void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("ClearTile"))
        {
            IsClear = true;
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
