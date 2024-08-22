using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ColorSelection : MonoBehaviour
{
    public GameObject[] objectsToChange; // Assign these in the Inspector
    public GameObject[] checkboxes; // Assign the checkbox GameObjects in the Inspector
    public GameObject[] colorRenders; // Assign the color render GameObjects in the Inspector
    public Material FaceMaterial;

    private Renderer[] objectRenderers;

    void Start()
    {
        objectRenderers = new Renderer[objectsToChange.Length];
        for (int i = 0; i < objectsToChange.Length; i++)
        {
            objectRenderers[i] = objectsToChange[i].GetComponent<Renderer>();
        }
    }

    public void OnClickColor(int index)
    {
        for (int i = 0; i < checkboxes.Length; i++)
        {
            checkboxes[i].SetActive(i == index); // Activate the checkbox at index, deactivate others
        }

        Color selectedColor = colorRenders[index].GetComponent<Image>().color;
        FaceMaterial.color = colorRenders[index].GetComponent<Image>().color;

        ApplyColor(selectedColor);
    }

    private void ApplyColor(Color color)
    {
        foreach (var renderer in objectRenderers)
        {
            renderer.material.color = color;
        }
    }
}
