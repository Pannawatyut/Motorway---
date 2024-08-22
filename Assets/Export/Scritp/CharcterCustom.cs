using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Linq;
using UnityEngine.XR;
using Unity.VisualScripting;

public class CharcterCustom : MonoBehaviour
{
    [SerializeField] private Renderer back;
    private Dictionary<string, string> colors = new Dictionary<string, string>()
    {
        {"Red", "E74C3C"},
        {"Orange", "E78A3C"},
        {"Yellow", "FFFF00"},
        {"Green", "27D491"},
        {"Blue", "4C3CE7"},
        {"Violet", "7A3CE7"},
        {"Gray", "7F7F7F"},
        {"White", "EEEEEE"},
    };
    // Start is called before the first frame update

    private int eyeID;
    private int mouthID;
    private int bodyColorID;
    private int screenColorID;

    [SerializeField] private Material[] eyes;

    [SerializeField] private TextMeshProUGUI eyeText;
    [SerializeField] private TextMeshProUGUI mouthText;
    [SerializeField] private TextMeshProUGUI bodyColorText;
    [SerializeField] private TextMeshProUGUI screenColorText;

    public void SelectBody(bool isForward)
    {
        if (isForward)
        {
            if (eyeID == eyes.Length - 1)
            {
                eyeID = 0;
            }
            else
            {
                eyeID++;
            }
        }
        else
        {
            if(eyeID == 0)
            {
                eyeID = eyes.Length - 1;
            }
            else
            {
                eyeID--;
            }
        }
        SetItem("eyes");
    }



    private void SetItem(string type)
    {
        switch(type)
        {
            case "eyes":
                eyeText.text = eyes[eyeID].name;
                break;
            case "mouths":
                mouthText.text = eyes[eyeID].name;
                break;
            case "screenColor":
                screenColorText.text = eyes[eyeID].name;
                break;
            case "bodyColor":
                bodyColorText.text = eyes[eyeID].name;
                break;
        }
    }

}
