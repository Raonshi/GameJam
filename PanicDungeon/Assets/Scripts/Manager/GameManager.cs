using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public string nextScene;



    private static GameManager instance;
    public bool isOpen;


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

    public void InitGameManager()
    {
        SoundManager.Singleton.InitSoundManager();
    }


    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        DoorOpen();
    }



    void DoorOpen()
    {
        if (isOpen == true)
        {
            isOpen = false;
            if (SceneManager.GetActiveScene().name == "Stage1")
            {
                LoadNextScene("Stage2");
            }
            else if(SceneManager.GetActiveScene().name == "Stage2")
            {
                LoadNextScene("Clear");
            }
        }
    }


    public void LoadNextScene(string sceneName)
    {
        nextScene = sceneName;

        SceneManager.LoadScene("Loading");

    }
}
