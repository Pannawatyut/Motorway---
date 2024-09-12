using System.Collections;
using UnityEngine;

public class TutorialScript : MonoBehaviour
{
#if !(UNITY_ANDROID || UNITY_IOS)

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

#endif
}
