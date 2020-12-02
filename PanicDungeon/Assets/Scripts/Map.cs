using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Map : MonoBehaviour
{
    public Tilemap floor;
    public GridLayout grid;
    public GameObject player;

    Vector3Int cellPos;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 point = player.transform.position;
        cellPos = grid.WorldToCell(point);

        floor.SetTileFlags(cellPos, TileFlags.None);
        floor.SetColor(cellPos, Color.green);
    }
}
