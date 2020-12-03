using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    public enum State
    {
        Idle,
        Line,
        Clear,
    }
    public State state;

    public bool isStart;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        switch(state)
        {
            //클리어 되지 않은 상태
            case State.Idle:
                return;
                break;
            //플레이어가 지나간 타일
            //선을 그려줘야함
            //플레이어는 주변 4방향의 타일을 탐색해서 가장 거리가 짧은 타일에 선을 그린다.
            case State.Line:
                
                break;

            //선 내부 타일은 색상 변경
            case State.Clear:

                break;
        }
    }
}
