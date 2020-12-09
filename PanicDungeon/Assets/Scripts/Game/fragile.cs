using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class fragile : Interactable
{
    public float SearchArea;
    [SerializeField] CircleCollider2D Area;
    BoxCollider2D Box;

    public SpriteRenderer image;
    private int check = 0;

    public override void Interact()
    {
        if (check == 0)
        {
            SoundManager.Singleton.PlaySound(Resources.Load<AudioClip>("Sounds/Effect_Object_Crash"));
            SetSearchArea();
            Invoke("DestroySearchArea", 0.5f);
            //DestroySearchArea();
            check++;
        }
    }

    //void OnTriggerEnter2D(Collider2D other)
    //{
    //    if (other.gameObject.CompareTag("Player"))
    //    {
    //        SetSearchArea();
    //        Invoke("DestroySearchArea", 1f);
    //    }
    //}

    void Start()
    {
        Area = GetComponent<CircleCollider2D>();
        Box = GetComponent<BoxCollider2D>();
        SearchArea = 0f;
        Area.enabled = false;
        image = gameObject.GetComponent<SpriteRenderer>();
        image.sprite = Resources.Load<Sprite>("Images/Interact/Interact_Dark_Idle");
    }

    void Update()
    {
        Area.radius = SearchArea;
    }

    void SetSearchArea()
    {
        SearchArea = 7f;        
        Area.enabled = true;
    }

    void DestroySearchArea()
    {
        StartCoroutine(ChangeSprite());

        SearchArea = 0f;
        Area.enabled = false;
        Box.enabled = false;
    }

    public void OpenInteractableIcon()
    {
        //InteractIcon.SetActive(true);
        Debug.Log("아이콘 생성");
    }

    public void CloseInteractableIcon()
    {
        //InteractIcon.SetActive(false);
        Debug.Log("아이콘 삭제");
    }

    private void OnDrawGizmos()
    {
        Vector3 originPos = transform.position;
        Gizmos.DrawWireSphere(originPos, SearchArea);
    }


    IEnumerator ChangeSprite()
    {
        image.sprite = Resources.Load<Sprite>("Images/Interact/Interact_Dark_Act");

        yield return new WaitForSeconds(0.5f);

        image.sprite = Resources.Load<Sprite>("Images/Interact/Interact_Dark_Broken");
    }
}
