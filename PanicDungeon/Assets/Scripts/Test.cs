using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test
{

    public int hp;
    public int mp;

    public enum Type
    {
        Type1,
        Type2,
        Type3,
    }

    public Type type;

    public void InitEnemy()
    {
        switch (type)
        {
            case Type.Type1:
                hp = 1;
                mp = 1;
                break;
            
            case Type.Type2:
                hp = 2;
                mp = 2;
                break;
            
            case Type.Type3:
                hp = 3;
                mp = 4;
                break;
        }
    }
}
