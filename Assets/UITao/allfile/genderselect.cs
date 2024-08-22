using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class genderselect : MonoBehaviour
{
    public GameObject[] gender;
    public GameObject[] Imagegender;

    public void _Genderselect(int index)
    {
        if (index >= 0 && index < gender.Length)
        {
            for (int i = 0; i < gender.Length; i++)
            {
                gender[i].SetActive(i == index);
                Imagegender[i].SetActive(i == index);
            }
        }
    }


}
