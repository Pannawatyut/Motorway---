using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class blackColor : MonoBehaviour
{
    private int bagID; // Index to track current material
    [SerializeField] private Material[] colorbag; // Array of materials to choose from
    [SerializeField] private GameObject black; // Reference to the GameObject whose material will be changed
    [SerializeField] private TextMeshProUGUI bagText; // Optional UI text to display current material name

    void Start()
    {
        // Initialize bagID to a random value within the range of colorbag length
    }

    // Method to set a specific material to the black GameObject
    private void SetMaterial(Material mat)
    {
        Renderer renderer = black.GetComponent<Renderer>();
        if (renderer != null)
        {
            renderer.material = mat;
        }
        else
        {
            Debug.LogError("Renderer component not found on black GameObject.");
        }
    }

    // Method to select the next or previous material in colorbag array
    public void SelectMaterial(bool isForward)
    {
        if (colorbag.Length == 0)
        {
            Debug.LogWarning("colorbag array is empty.");
            return;
        }

        if (isForward)
        {
            bagID = (bagID + 1) % colorbag.Length; // Wrap around to beginning if at end
        }
        else
        {
            bagID = (bagID - 1 + colorbag.Length) % colorbag.Length; // Wrap around to end if at beginning
        }

        SetMaterial(colorbag[bagID]); // Set the selected material
        UpdateBagText(); // Update UI text if available
    }

    // Method to update UI text with the current material name
    private void UpdateBagText()
    {
        if (bagText != null && colorbag.Length > 0)
        {
            bagText.text = colorbag[bagID].name;
        }
    }
}
