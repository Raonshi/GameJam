using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LineEnergy : MonoBehaviour
{
    public Slider slider;
    public Text text;

    // Start is called before the first frame update
    void Start()
    {
        slider.maxValue = 100;
        slider.value = Game.instance.clearRate;
    }

    // Update is called once per frame
    void Update()
    {
        slider.value = Game.instance.clearRate;
        text.text = string.Format("{0} %", (int)slider.value);
    }
}
