using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Blink : MonoBehaviour
{
    public Text blinkText;
    // Start is called before the first frame update
    void Start()
    {
        GameManager.Singleton.InitGameManager();
        blinkText.text = "PRESS  ANY  KEY  TO  START";
        blinkText.fontSize = 20;
        SoundManager.Singleton.PlaySound(Resources.Load<AudioClip>("Sounds/BGM_Main"));
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.anyKeyDown)
        {
            GameManager.Singleton.LoadNextScene("Stage1");
            SoundManager.Singleton.SoundInit();
        }
    }
}
