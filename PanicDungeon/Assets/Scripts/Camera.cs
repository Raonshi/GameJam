using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Camera : MonoBehaviour
{
    Camera camera;
    public GameObject mainCamera;
    
    public Transform player;
    
    public BoxCollider2D collider;
    private Vector3 min;
    private Vector3 max;
    private float width;
    private float height;
    
    
    // Start is called before the first frame update
    void Start()
    {
        camera = gameObject.GetComponent<Camera>();

        min = collider.bounds.min;
        max = collider.bounds.max;
        
        height = camera.height;
        width = height * Screen.width / Screen.height;


    }

    // Update is called once per frame
    void Update()
    {
        ChasingPlayer();
    }

    public void ChasingPlayer()
    {
        transform.position = new Vector3(player.position.x, player.position.y, -10);
        
        float clampX = Mathf.Clamp(transform.position.x, min.x + width, max.x - width);
        float clampY = Mathf.Clamp(transform.position.y, min.y + height, max.y - height);
        
        transform.position = new Vector3(clampX, clampY, -10);
    }
}
