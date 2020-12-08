using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public string nextScene;



    private static GameManager instance;


    public static GameManager Singleton
    {
        get
        {
            if(instance == null)
            {
                GameObject obj = GameObject.Find("GameManager");

                if(obj == null)
                {
                    obj = new GameObject("GameManager");
                    obj.AddComponent<GameManager>();
                    DontDestroyOnLoad(obj);
                }

                instance = obj.GetComponent<GameManager>();
            }

            return instance;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public void LoadNextScene(string sceneName)
    {
        nextScene = sceneName;

        SceneManager.LoadScene("Loading");

    }
}
