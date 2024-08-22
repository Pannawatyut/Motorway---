using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SettingUI : MonoBehaviour
{
    public GameObject SettingUi;
    public static bool turnOff =false;

    public GameObject Button_ConfirmSetting;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
       
    }

    public void SaveSetting()
    {
        Button_ConfirmSetting.SetActive(false);
        SettingUi.SetActive(true);
    }
}
