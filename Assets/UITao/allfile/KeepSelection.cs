using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;

public class KeepSelection : MonoBehaviour, ISelectHandler, IDeselectHandler
{
    private bool keepSelected = false;

    public void OnSelect(BaseEventData eventData)
    {
        keepSelected = true;
    }

    public void OnDeselect(BaseEventData eventData)
    {
        if (keepSelected)
        {
            StartCoroutine(DelayedDeselect());
        }
    }

    private IEnumerator DelayedDeselect()
    {
        yield return null; // ????????????????????????? EventSystem ?????????????????????????????????

        if (keepSelected)
        {
            EventSystem.current.SetSelectedGameObject(gameObject);
        }
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            keepSelected = false;
        }
    }
}
