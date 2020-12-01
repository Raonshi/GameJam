using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Type1 : MonoBehaviour
{
    public int hp;
    public int mp;
    
    public Test test = new Test();
    // Start is called before the first frame update
    void Start()
    {
        test.type = Test.Type.Type3;    //test객체의 Type은 Type1이다.
        test.InitEnemy();               //Type1의 값을 참조하여, InitEnemy를 실행

        hp = test.hp;
        mp = test.mp;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
