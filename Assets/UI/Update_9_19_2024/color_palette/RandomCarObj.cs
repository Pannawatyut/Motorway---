using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomCarObj : MonoBehaviour
{

    public List<GameObject> cars = new List<GameObject>();
    public void OnEnable()
    {
        foreach(GameObject x in cars)
        {
            x.SetActive(false);
        }

        cars[Random.Range(0, cars.Count)].SetActive(true);
    }
}
