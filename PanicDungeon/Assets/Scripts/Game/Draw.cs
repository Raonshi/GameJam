using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Draw : MonoBehaviour
{
    public List<GameObject> lineList = new List<GameObject>();
    public static Draw instance;

    private void Awake()
    {
        instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
           
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    /*
 * 선 그리기
 * 1. isDraw의 상태가 true일 경우에만 동작한다.
 * 2. isDraw인 경우에만 선이 그려지고, isDraw가 false가 되면 선이 모두 없어진다.
 * 3. 
 */

    public void Drawing()
    {
        if(Player.instance.isDraw == false)
        {
            return;
        }

        float time = 0;

        while (time > 1.0f)
        {
            time += Time.deltaTime;
        }

        Vector3 tilePos = 

        if(lineList.Count == 0)
        {

        }


        Player.instance.lineEnergy -= Time.deltaTime;
        Debug.Log("Line!");
    }


    public void OnTriggerEnter2D(Collider2D other)
    {
        if()
    }
}
