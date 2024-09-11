using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class TutorialScript : MonoBehaviour
{
    public GameObject TutorialPage;
    public GameObject TutorialGUI;
    private void OnEnable()
    {
        StartCoroutine(DelaybeforeAutoClose());
    }
    IEnumerator DelaybeforeAutoClose()
    {
        yield return new WaitForSeconds(3f);
        TutorialPage.SetActive(false);
        TutorialGUI.SetActive(true);
    }
}
