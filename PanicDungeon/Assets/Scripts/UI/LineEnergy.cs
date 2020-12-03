using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LineEnergy : MonoBehaviour
{
    public Slider slider;
    public Text text;

    public Player player;

    // Start is called before the first frame update
    void Start()
    {
        slider.maxValue = player.maxLineEnergy;
    }

    // Update is called once per frame
    void Update()
    {
        slider.value = player.lineEnergy;
        text.text = string.Format("{0}/{1}", (int)slider.value, slider.maxValue);
    }
}
