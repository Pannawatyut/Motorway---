using UnityEngine;
using TMPro;
using UnityEngine.EventSystems;

public class TabNavigation : MonoBehaviour
{
    public TMP_InputField[] inputFields; // Drag and drop your TMP InputFields in the inspector
    private int currentInputIndex = 0;

    void Start()
    {
        if (inputFields.Length > 0)
        {
            // Select the first input field initially
            EventSystem.current.SetSelectedGameObject(inputFields[0].gameObject);
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            // If Shift is also held, move to the previous field
            if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift))
            {
                MoveToPreviousInput();
            }
            else
            {
                MoveToNextInput();
            }
        }
    }

    void MoveToNextInput()
    {
        currentInputIndex++;
        if (currentInputIndex >= inputFields.Length)
        {
            currentInputIndex = 0; // Loop back to the first input field
        }
        SelectInputField(currentInputIndex);
    }

    void MoveToPreviousInput()
    {
        currentInputIndex--;
        if (currentInputIndex < 0)
        {
            currentInputIndex = inputFields.Length - 1; // Loop back to the last input field
        }
        SelectInputField(currentInputIndex);
    }

    void SelectInputField(int index)
    {
        TMP_InputField inputField = inputFields[index];
        inputField.Select(); // Selects the input field
        inputField.ActivateInputField(); // Ensures the input field is ready for typing
        EventSystem.current.SetSelectedGameObject(inputField.gameObject); // Set it in EventSystem
    }
}
