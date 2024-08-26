using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class loadSceenManager : MonoBehaviour
{
    public GameObject loadScreenAnimator;
    private Animator animator;

    void Start()
    {
        if (loadScreenAnimator != null)
        {
            animator = loadScreenAnimator.GetComponent<Animator>();
            if (animator != null)
            {
                AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);
                StartCoroutine(WaitForAnimationToEnd(stateInfo.length));
            }
            else
            {
                Debug.LogError("No Animator component found on loadScreenAnimator.");
            }
        }
        else
        {
            Debug.LogError("loadScreenAnimator is not assigned.");
        }
    }

    private IEnumerator WaitForAnimationToEnd(float animationLength)
    {
        yield return new WaitForSeconds(animationLength);
        yield return new WaitForSeconds(2);
        LoadNextScene();
    }

    private void LoadNextScene()
    {
        SceneManager.LoadScene(2);
    }
}
