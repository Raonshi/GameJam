using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Tile : MonoBehaviour
{
    public bool isStart = false;
    public SpriteRenderer sprite;
    public BoxCollider2D boxCollider;
    public Animator anim;
    public GameObject startPoint;

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
        gameObject.AddComponent<Animator>();
        sprite = gameObject.GetComponent<SpriteRenderer>();
        boxCollider = gameObject.GetComponent<BoxCollider2D>();
        anim = gameObject.GetComponent<Animator>();
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
                if(isStart == true)
                {
                    if(gameObject.transform.childCount != 0)
                    {
                        return;
                    }

                    startPoint = new GameObject("StartPoint");

                    startPoint.transform.SetParent(gameObject.transform);
                    startPoint.transform.position = gameObject.transform.position;

                    startPoint.AddComponent<SpriteRenderer>();                 
                    startPoint.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Images/Tile/Blue_flag");
                    startPoint.GetComponent<SpriteRenderer>().enabled = true;
                }
                //sprite.sprite = Resources.Load<Sprite>("Line");
                //sprite.size = new Vector2(100, 100);
                anim.runtimeAnimatorController = Resources.Load<RuntimeAnimatorController>("Animation/Clear_Tile_0");
                break;

            case State.CLEAR:
                //anim.enabled = false;
                //sprite.sprite = Resources.Load<Sprite>("Line");
                //sprite.size = new Vector2(100, 100);
                if (gameObject.transform.childCount != 0)
                {
                    startPoint.GetComponent<SpriteRenderer>().enabled = false;
                }               
                anim.runtimeAnimatorController = Resources.Load<RuntimeAnimatorController>("Animation/Clear_Tile_0");
                gameObject.tag = "ClearTile";
                boxCollider.enabled = true;
                break;

            case State.OUTSIDE:
                anim.runtimeAnimatorController = null;
                sprite.sprite = null;
                break;
        }
    }
}
