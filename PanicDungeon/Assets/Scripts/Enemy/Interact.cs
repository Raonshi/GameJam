using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interact : MonoBehaviour
{
    public float SearchArea = 0f;
    [SerializeField] CircleCollider2D Area;
    BoxCollider2D Box;

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

    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject.CompareTag("Player"))
        {
            Debug.Log("플레이어 인식");
            SetSearchArea();
            Invoke("DestroySearchArea", 5f);
        }

        if (other.gameObject.CompareTag("Enemy"))
        {
            Debug.Log("적 인식");
        }
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

    private void OnDrawGizmos()
    {
        Vector3 originPos = transform.position;
        Gizmos.DrawWireSphere(originPos, SearchArea);
    }
}
