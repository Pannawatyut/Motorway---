using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangCanvas : MonoBehaviour
{
    // Array to hold the button game objects
    public GameObject[] ButtonPressF;

    // Start is called before the first frame update
#if UNITY_ANDROID
    void Start()
    {
        ButtonPressF[1].SetActive(true);
        ButtonPressF[0].SetActive(false);
    }
#else
    void Start()
    {
        ButtonPressF[0].SetActive(true);
        ButtonPressF[1].SetActive(false);
    }
#endif
}
