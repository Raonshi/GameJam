using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class fragile : Interactable
{
    public float SearchArea = 0f;
    [SerializeField] CircleCollider2D Area;
    BoxCollider2D Box;

    public override void Interact()
    {
        SetSearchArea();
        Invoke("DestroySearchArea", 1f);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            SetSearchArea();
            Invoke("DestroySearchArea", 1f);
        }
    }

    void Start()
    {
        Area = GetComponent<CircleCollider2D>();
        Box = GetComponent<BoxCollider2D>();
        Area.enabled = false;
    }

    void Update()
    {
        Area.radius = SearchArea;
    }

    void SetSearchArea()
    {
        SearchArea = 15f;
        Area.enabled = true;
    }

    void DestroySearchArea()
    {
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
}
