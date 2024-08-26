using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SexSelect : MonoBehaviour
{
    public GameObject[] Sex;
    public GameObject[] Player;
    public GameObject PlayerList;

    public void OnMan()
    {
        Sex[0].SetActive(true);
        Sex[1].SetActive(false);
        Player[0].SetActive(true);
        Player[1].SetActive(false);
        PlayerList = Player[0].GetComponent<GameObject>();

    }

    public void OnWomen()
    {
        Sex[0].SetActive(false);
        Player[0].SetActive(false);
        Sex[1].SetActive(true);
        Player[1].SetActive(true);
        PlayerList = Player[1].GetComponent<GameObject>();
    }
}
