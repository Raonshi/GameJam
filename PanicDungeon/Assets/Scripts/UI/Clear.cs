using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Clear : MonoBehaviour
{
    public Text blinkText;
    // Start is called before the first frame update
    void Start()
    {
        GameManager.Singleton.InitGameManager();
        blinkText.text = "C L E A R ! ! !";
        blinkText.fontSize = 60;
        SoundManager.Singleton.PlaySound(Resources.Load<AudioClip>("Sounds/BGM_Main"));
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.anyKeyDown)
        {
            GameManager.Singleton.LoadNextScene("Stage1");
            SoundManager.Singleton.SoundInit();
        }
    }
}
