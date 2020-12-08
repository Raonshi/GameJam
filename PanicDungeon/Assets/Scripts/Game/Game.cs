using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class Game : MonoBehaviour
{
    //public int tileWidthCount;
    //public int tileHeightCount;
    //public float tileSize;

    ////public List<Vector2> tilePos = new List<Vector2>();
    //public List<Tile> tileList = new List<Tile>();

    //public static Game instance;

    //private void Awake()
    //{
    //    instance = this;
    //}

    //// Start is called before the first frame update
    //void Start()
    //{
    //    InitTilePosition();
    //}

    //// Update is called once per frame
    //void Update()
    //{

    //}

    //public void InitTilePosition()
    //{
    //    for (int y = 0; y < tileHeightCount; y++)
    //    {
    //        for (int x = 0; x < tileWidthCount; x++)
    //        {
    //            GameObject obj = Instantiate(Resources.Load<GameObject>("Prefabs/Tile"));
    //            obj.transform.SetParent(GameObject.Find("InGame/Map/TilePos").transform);
    //            obj.transform.position = new Vector2(-8.5f + (tileSize * x), 6.5f - (tileSize * y));

    //            tileList.Add(obj.GetComponent<Tile>());
    //        }
    //    }
    //}

    /*
     * 타일 변화 로직
     * 1. Update()에 작성되어 매 프레임마다 호출된다.
     * 2. Player.cs가 Game.cs로 검사가 가능하다고 알린다.
     * 3. 검사가 가능할 경우 검사를 시작하고, 가능하지 않으면 검사하지 않는다.
     * 4. 검사가 시작되면 모든 타일의 정보를 순차적으로 탐색한다.
     * 5. 타일의 상태가 OUTSIDE면 건너뛴다.
     * 6. 타일의 상태가 INSIDE면 타일에 비활성화 되어 있던 클리어 이미지를 활성화한다.
     */

    public float xPos;
    public float yPos;
    public float tileGap;
    public int xSize;
    public int ySize;

    public int clearRate;

    public List<Transform> tileList = new List<Transform>();

    public List<Tile> clearList = new List<Tile>();

    public GameObject tilePos;


    public enum Direction
    {
        NULL,
        UP,
        DOWN,
        LEFT,
        RIGHT,
    }
    public Direction direction;

    public int count;

    public static Game instance;

    private void Awake()
    {
        instance = this;
        direction = Direction.NULL;
    }

    // Start is called before the first frame update
    void Start()
    {
        for (int x = 0; x < xSize; x++)
        {
            for (int y = 0; y < ySize; y++)
            {
                GameObject obj = new GameObject("Tile");
                obj.transform.SetParent(tilePos.transform);
                obj.transform.position = new Vector3(xPos + (x * tileGap), yPos + (y * tileGap), 0);
                obj.AddComponent<Tile>();

                tileList.Add(obj.transform);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (direction != Player.instance.direction)
        {
            count++;
            direction = Player.instance.direction;
        }
    }

    public void Clear()
    {
        for (int i = 0; i < clearList.Count; i++)
        {
            clearList[i].state = Tile.State.CLEAR;
        }
        
        ClearCheck();
    }
    
    public void ClearCheck()
    {
        float checkCount = tileList.Count;
        float clearCount = clearList.Count;
        clearRate = Convert.ToInt32((clearCount / checkCount) * 100);
        
        if (clearRate >= 80 || Player.instance.hasKey == true)
        {
            Debug.Log("Clear");
            GameManager.Singleton.LoadNextScene("Shop");
        }
        Debug.Log(clearRate);
    }
}
