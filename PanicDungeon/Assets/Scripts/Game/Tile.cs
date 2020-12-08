﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Tile : MonoBehaviour
{
    public bool isStart;
    public SpriteRenderer sprite;
    public BoxCollider2D boxCollider;

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
        gameObject.AddComponent<BoxCollider2D>();
        sprite = gameObject.GetComponent<SpriteRenderer>();
        boxCollider = gameObject.GetComponent<BoxCollider2D>();
        boxCollider.size = new Vector2(1f, 1f);
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
                sprite.sprite = Resources.Load<Sprite>("Line");
                sprite.size = new Vector2(100, 100);
                break;
            case State.CLEAR:
                sprite.sprite = Resources.Load<Sprite>("Line");
                sprite.size = new Vector2(100, 100);
                gameObject.tag = "ClearTile";
                boxCollider.enabled = true;
                break;

            case State.OUTSIDE:
                sprite.sprite = null;
                break;
        }
    }
}
