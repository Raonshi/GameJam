using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    void Start()
    {
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
                Debug.Log("추격");
                break;
            case EModule.EnemyState.patrol:
                Debug.Log("순찰");
                yield return new WaitForSeconds(0.2f);
                break;
        }
    }
}
