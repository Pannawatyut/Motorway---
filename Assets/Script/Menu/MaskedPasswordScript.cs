using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

[RequireComponent(typeof(TMP_InputField))]
public class MaskedPasswordScript : MonoBehaviour
{
    private TMP_InputField inputField;
    public string actualInput = "";
    private void OnValueChanged(string text)
    {
        // Update the actual input string
        if (text.Length > actualInput.Length)
        {
            // Character added
            actualInput += text[text.Length - 1];
        }
        else if (text.Length < actualInput.Length)
        {
            // Character removed
            actualInput = actualInput.Substring(0, actualInput.Length - 1);
        }

        // Mask the displayed text with asterisks
        inputField.text = new string('*', actualInput.Length);
    }

    // Function to mask the displayed text
    private void MaskPassword()
    {
        inputField.text = new string('*', actualInput.Length);
        inputField.MoveTextEnd(false); // Ensure cursor stays at the end of the input
    }

    // Optionally, provide a method to retrieve the actual input
    public string GetPassword()
    {
        return actualInput;
    }

    // Called when the GameObject or component is enabled
    private void OnEnable()
    {
        inputField = GetComponent<TMP_InputField>();
        inputField.contentType = TMP_InputField.ContentType.Password;
        inputField.onValueChanged.AddListener(OnValueChanged);

        if (!string.IsNullOrEmpty(actualInput))
        {
            MaskPassword(); // Ensure the password is masked when the component is enabled
        }
    }
}
