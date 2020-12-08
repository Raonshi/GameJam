using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EModule
{
    public float WalkSpeed;
    public float RunSpeed;

    public enum Type
    {
        Enemy1,
        Enemy2,
        Enemy3
    }

    public enum EnemyState
    {
        patrol, trace, caution, dead
    }

    public Type type;
    public EnemyState state = EnemyState.patrol;

    public void InitEnemy()
    {
        switch (type)
        {
            case Type.Enemy1:
                WalkSpeed = 1f;
                RunSpeed = 2f;
                break;
            case Type.Enemy2:
                WalkSpeed = 2f;
                RunSpeed = 3f;
                break;
            case Type.Enemy3:
                WalkSpeed = 2f;
                RunSpeed = 4f;
                break;
        }
    }
}
