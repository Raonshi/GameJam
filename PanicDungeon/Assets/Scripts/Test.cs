using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
               
    }

    // Update is called once per frame
    void Update()
    {
        if(gameObject.GetComponent<SpriteRenderer>().sprite == null)
        {
            Debug.Log("NEE!!!");
        }
        else if (gameObject.GetComponent<SpriteRenderer>().enabled == false)
        {
            Debug.Log("YEE!!!");
        }
        else
        {
            Debug.Log("HEE!!!");
        }
    }
}
