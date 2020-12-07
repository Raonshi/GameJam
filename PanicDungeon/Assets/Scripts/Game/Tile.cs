using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Tile : MonoBehaviour
{
    public bool isStart;
    public SpriteRenderer sprite;
    public TilemapCollider2D boxCollider;
    public Rigidbody2D rigid;

    public enum State
    {
        INSIDE,
        OUTSIDE,
        CLEAR,
    }

    public State state;

    // Start is called before the first frame update
    void Start()
    {
        state = State.OUTSIDE;
        gameObject.AddComponent<SpriteRenderer>();
        gameObject.AddComponent<TilemapCollider2D>();
        sprite = gameObject.GetComponent<SpriteRenderer>();
        boxCollider = gameObject.GetComponent<TilemapCollider2D>();
        boxCollider.isTrigger = true;
        boxCollider.enabled = false;

        sprite.sortingLayerName = "Map";
    }

    // Update is called once per frame
    void Update()
    {
        switch (state)
        {
            case State.INSIDE:
            case State.CLEAR:
                sprite.sprite = Resources.Load<Sprite>("Line");
                sprite.size = new Vector2(100, 100);
                gameObject.tag = "ClearTile";
                gameObject.AddComponent<Rigidbody2D>();
                rigid = gameObject.GetComponent<Rigidbody2D>();
                rigid.bodyType = RigidbodyType2D.Static;
                boxCollider.enabled = true;
                break;

            case State.OUTSIDE:
                sprite.sprite = null;
                break;
        }
    }

    void OnTriggerStay2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
            Debug.Log("적 사망 가능");
            //IsClear = true;
        }
    }
}
