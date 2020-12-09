using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class Game : MonoBehaviour
{
    public float xPos;
    public float yPos;
    public float tileGap;
    public int xSize;
    public int ySize;
    public static int check;
    public int clearRate;

    public List<Transform> tileList = new List<Transform>();

    public List<Tile> clearList = new List<Tile>();

    public GameObject tilePos;

    public GameObject[] enemies;

    public GameObject door;


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
        door = GameObject.FindGameObjectWithTag("Door");
        door.SetActive(false);
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

        enemies = GameObject.FindGameObjectsWithTag("Enemy");
        enemies[UnityEngine.Random.Range(0, enemies.Length)].GetComponent<Enemy>().hasKey = true;

        check++;
        if(check == 1)
            SoundManager.Singleton.PlaySound(Resources.Load<AudioClip>("Sounds/BGM_Stage"));
    }

    // Update is called once per frame
    void Update()
    {
        if (direction != Player.instance.direction)
        {
            count++;
            direction = Player.instance.direction;
        }

        if(clearRate >= 80)
        {
            EnemyDelete();
        }

        ClearCheck();
    }

    public void Clear()
    {
        for (int i = 0; i < clearList.Count; i++)
        {
            SoundManager.Singleton.PlaySound(Resources.Load<AudioClip>("Sounds/Effect_Qix"));
            clearList[i].state = Tile.State.CLEAR;
        }
       
    }
    
    public void ClearCheck()
    {
        float checkCount = tileList.Count;
        float clearCount = clearList.Count;
        clearRate = Convert.ToInt32((clearCount / checkCount) * 100);

        if (Player.instance.hasKey == true)
        {
            Debug.Log("Clear");
            door.SetActive(true);
        }
        Debug.Log(clearRate);
    }

    public void EnemyDelete()
    {
        GameObject[] array = GameObject.FindGameObjectsWithTag("Enemy");

        for(int i = 0; i < array.Length; i++)
        {
            array[i].GetComponent<Enemy>().emodule.state = EModule.EnemyState.dead;
        }
    }
}
