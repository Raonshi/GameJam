using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Enemy : MonoBehaviour
{
    public EModule emodule = new EModule();
    EnemyFOV enemyFOV;
    Player player;
    [SerializeField] Transform Tr;
    [SerializeField] Transform playerTr;
    [SerializeField] Transform position;
    public Game game;

    public GameObject key;

    public Transform target;
    public Transform destination;
    public GameObject[] destinationList;
    public List<GameObject> desto = new List<GameObject>();

    public GameObject trail;

    public float WalkSpeed;
    public float RunSpeed;
    public bool isKnow;
    public bool IsClear;
    public bool isIdle;

    public float time = 3.0f;
    public float rotation;
    public bool hasKey;
    int stack = 0;

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
        destinationList = GameObject.FindGameObjectsWithTag("PatrolPoint");
        desto = destinationList.ToList();

        isKnow = false;
        isIdle = false;
        position = null;
        game = Game.instance;
        IsClear = false;
        hasKey = false;


        key.SetActive(false);
    }

    void FixedUpdate()
    {
        time -= Time.deltaTime;

        StartCoroutine(CheckEnemyState());
        StartCoroutine(ActionEnemy());
        if(game.clearList.Count > 0)
            CheckEscape();
    }

    IEnumerator CheckEnemyState()
    {
        yield return new WaitForSeconds(0.1f);
        if (IsClear)
            emodule.state = EModule.EnemyState.dead;
        else if (enemyFOV.IsView)
            emodule.state = EModule.EnemyState.trace;
        else if (!enemyFOV.IsView && isKnow)
            emodule.state = EModule.EnemyState.caution;       
        else
            emodule.state = EModule.EnemyState.patrol;
    }

    IEnumerator ActionEnemy()
    {
        switch (emodule.state)
        {
            case EModule.EnemyState.trace:
                if (Player.instance.isDash == true)
                {
                    Player.instance.speed = Player.instance.dashSpeed * 0.6f;
                }
                else
                {
                    Player.instance.speed = Player.instance.moveSpeed * 0.6f;
                }

                UpdateDirection();
                transform.position = Vector3.MoveTowards(transform.position, target.position, RunSpeed * Time.deltaTime);
                isKnow = false;
                break;

            case EModule.EnemyState.patrol:
                if(Player.instance.isDash == true)
                {
                    Player.instance.speed = Player.instance.dashSpeed;
                }
                else
                {
                    Player.instance.speed = Player.instance.moveSpeed;
                }

                //Patrol();
                yield return null;
                break;

            case EModule.EnemyState.caution:
                transform.position = Vector2.MoveTowards(transform.position, position.position, WalkSpeed * Time.deltaTime);
                float dist = Vector2.Distance(transform.position, position.position);
                if(dist <= 1f)
                {
                    UnEnableIsKnow();
                }              
                break;

            case EModule.EnemyState.dead:

                if(hasKey)
                {
                    key.SetActive(true);
                    key.transform.position = transform.position;
                }             
                
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
        Vector3 dist = Vector3.zero;
        

        if (destination == null)
        {
            //destination = desto[UnityEngine.Random.Range(0, desto.Count)].GetComponent<Transform>();
            destination = desto[0].GetComponent<Transform>();
        }
        else
        {
            while (true)
            {
                for (int i = 0; i < game.clearList.Count; i++)
                {
                    for (int k = 0; k < desto.Count; k++)
                    {
                        if (desto[k].GetComponent<Transform>().position == game.clearList[i].GetComponent<Transform>().position)
                        {
                            Debug.Log(desto[k]);
                            desto.RemoveAt(k);
                        }
                    }
                }
                break;
            }
            if (Vector3.Distance(transform.localPosition, destination.localPosition) <= 1f)
            {              
                WalkSpeed = UnityEngine.Random.Range(1.0f, 3.0f);
                if (stack == desto.Count)
                    stack = 0;
                destination = desto[stack].GetComponent<Transform>();
                stack += 1;
            }
            else
            {
                dist = transform.localPosition - destination.localPosition;
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
                transform.position = Vector3.MoveTowards(transform.position, destination.position, WalkSpeed * Time.deltaTime);                
            }
        }
    }

    public void CheckEscape()
    {
        float fistDist = Vector2.Distance(transform.position, game.clearList[0].GetComponent<Transform>().position);
        int firstTile = 0;
        for (int i = 0; i < game.clearList.Count; i++)
        {
            float dist = Vector2.Distance(transform.position, game.clearList[i].GetComponent<Transform>().position);
            if (dist < fistDist)
            {
                fistDist = dist;
                firstTile = i;
            }                
        }

        for (int i = 0; i < game.clearList.Count; i++)
        {
            float distX = transform.position.x - game.clearList[firstTile].GetComponent<Transform>().position.x;
            float distY = transform.position.y - game.clearList[firstTile].GetComponent<Transform>().position.y;

            float distXY = Mathf.Abs(distX) - Math.Abs(distY); //결과값이 +인 경우 X좌표 우선 계산, -인 경우 Y좌표 우선 계산

            if (distX <= 1.5f && distX > 0 && distXY > 0)
            {
                destination = desto[UnityEngine.Random.Range(0, desto.Count)].GetComponent<Transform>();
                //Debug.Log("오른쪽으로 도망");
                intoPatrol();
            }
            else if (distX >= -1.5f && distX < 0 && distXY > 0)
            {
                destination = desto[UnityEngine.Random.Range(0, desto.Count)].GetComponent<Transform>();
                //Debug.Log("왼쪽으로 도망");
                intoPatrol();
            }
            if (distY <= 1.5f && distY > 0 && distXY < 0)
            {
                destination = desto[UnityEngine.Random.Range(0, desto.Count)].GetComponent<Transform>();
                //Debug.Log("위쪽으로 도망");
                intoPatrol();
            }
            else if (distY >= -1.5f && distY < 0 && distXY < 0)
            {
                destination = desto[UnityEngine.Random.Range(0, desto.Count)].GetComponent<Transform>();
                //Debug.Log("아래쪽으로 도망");
                intoPatrol();
            }
        }
    }

    public void intoPatrol()
    {
        isKnow = false;
        enemyFOV.IsView = false;
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
