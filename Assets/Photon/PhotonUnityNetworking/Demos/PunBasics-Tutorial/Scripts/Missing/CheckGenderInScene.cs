using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckGenderInScene : MonoBehaviour
{
    public GameObject Woman;
    public GameObject Man;
    public GameObject RandomWoman;
    public GameObject RandomMen;
    
    
    public GameObject EnterWoman;
    public GameObject EnterMen;
    private void Update()
    {
        if (Woman.gameObject.activeInHierarchy)
        {
            RandomWoman.gameObject.SetActive(true);
            EnterWoman.gameObject.SetActive(true);
        }
        else
        {
            RandomWoman.gameObject.SetActive(false);
            EnterWoman.gameObject.SetActive(false);
        }
        
        if (Man.gameObject.activeInHierarchy)
        {
            RandomMen.gameObject.SetActive(true);
            EnterMen.gameObject.SetActive(true);
        }
        else
        {
            RandomMen.gameObject.SetActive(false);
            EnterMen.gameObject.SetActive(false);
        }
        
    }
}
