using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Game : MonoBehaviour
{
    public int tileWidthCount;
    public int tileHeightCount;
    public float tileSize;

    //public List<Vector2> tilePos = new List<Vector2>();
    public List<Tile> tileList = new List<Tile>();

    public static Game instance;

    private void Awake()
    {
        instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        InitTilePosition();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void InitTilePosition()
    {
        for (int y = 0; y < tileHeightCount; y++)
        {
            for (int x = 0; x < tileWidthCount; x++)
            {
                GameObject obj = Instantiate(Resources.Load<GameObject>("Prefabs/Tile"));
                obj.transform.SetParent(GameObject.Find("InGame/Map/TilePos").transform);
                obj.transform.position = new Vector2(-8.5f + (tileSize * x), 6.5f - (tileSize * y));

                tileList.Add(obj.GetComponent<Tile>());
            }
        }
    }
}
