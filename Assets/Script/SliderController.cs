using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SliderController : MonoBehaviour
{
    public static SliderController Instance { get; private set; }
    public float Carsindex = 0;

    public Slider slider;

    void Update()
    {
        slider.value = Carsindex;
    }
    // Start is called before the first frame update
    public void UpdateSlider(float CarCount)
    {
        Carsindex = CarCount;
    }


}
