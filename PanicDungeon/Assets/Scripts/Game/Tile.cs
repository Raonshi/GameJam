using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    public bool isStart;
    public SpriteRenderer sprite;

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
        sprite = gameObject.GetComponent<SpriteRenderer>();

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
                break;

            case State.OUTSIDE:
                sprite.sprite = null;
                break;
        }
    }
}
