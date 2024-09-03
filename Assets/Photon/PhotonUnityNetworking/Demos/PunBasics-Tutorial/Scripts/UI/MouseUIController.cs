using UnityEngine;

public class MouseUIController : MonoBehaviour
{
    public float scrollMouseSpeed = 100f;  // Speed at which zooming occurs
    public float minZoom = 100f;  // Minimum allowed size
    public float maxZoom = 800f;  // Maximum allowed size
    public float dragSpeed = 0.5f; // Speed factor for dragging, lower means slower dragging

    public RectTransform uiRectTransform;  // Use RectTransform for the panel
    public float zoomSpeed = 0.1f; // Speed factor for zooming, lower means slower zooming

    private bool isFollowingMouse = true; // Flag to control whether the UI follows the mouse
    private bool isDragging = false; // Flag to control dragging state
    private Vector2 dragOffset; // Offset to maintain the relative position of the panel during dragging
    public GameObject isOn; // GameObject to indicate dragging status

private Vector2 defaultPosition; // Default position of the UI element
    private Vector2 defaultSize; // Default size of the UI element
    private void Start()
    {
        if (uiRectTransform == null)
        {
            uiRectTransform = GetComponent<RectTransform>();
        }

        // Initialize default position and size
        defaultPosition = uiRectTransform.anchoredPosition;
        defaultSize = uiRectTransform.sizeDelta;
    }

    private void Update()
    {
        if (isFollowingMouse)
        {
            HandleDrag();
        }

        // Check for mouse scroll input and adjust zoom level
        float scrollInput = Input.GetAxis("Mouse ScrollWheel");
        if (scrollInput != 0f)
        {
            AdjustZoom(scrollInput);
        }
    }

    private void HandleDrag()
    {
        if (Input.GetMouseButtonDown(0)) // Left mouse button down
        {
            isDragging = true;
            //isOn.gameObject.SetActive(false);
            // Calculate the drag offset in local space
            Vector2 localMousePosition;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(uiRectTransform, Input.mousePosition, null, out localMousePosition);
            dragOffset = uiRectTransform.anchoredPosition - localMousePosition;
        }

        if (Input.GetMouseButtonUp(0)) // Left mouse button up
        {
            isDragging = false;
            //isOn.gameObject.SetActive(true);

        }

        if (isDragging)
        {
            Vector2 localMousePosition;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(uiRectTransform, Input.mousePosition, null, out localMousePosition);
            uiRectTransform.anchoredPosition = Vector2.Lerp(uiRectTransform.anchoredPosition, localMousePosition + dragOffset, dragSpeed);
        }
    }

    private void AdjustZoom(float scrollInput)
    {
        if (uiRectTransform != null)
        {
            // Calculate zoom change based on scroll input and camSpeed
            float zoomChange = scrollInput * scrollMouseSpeed;

            // Calculate new size based on zoomChange
            Vector2 currentSize = uiRectTransform.sizeDelta;
            Vector2 newSize = currentSize + new Vector2(zoomChange, zoomChange);

            // Clamp size to be within min and max zoom levels
            newSize.x = Mathf.Clamp(newSize.x, minZoom, maxZoom);
            newSize.y = Mathf.Clamp(newSize.y, minZoom, maxZoom);

            // Apply the new size to the RectTransform
            if (newSize != currentSize)
            {
                uiRectTransform.sizeDelta = newSize;
            }
        }
        else
        {
            Debug.LogError("uiRectTransform is not assigned!");
        }
    }

    public void ToggleMouseFollow()
    {
        isFollowingMouse = !isFollowingMouse;   
    }

    public void OnButtonClick()
    {
        ZoomIn();
    }

    public void OnButtonShrink()
    {
        ZoomOut();
    }

    private void ZoomIn()
    {
        if (uiRectTransform != null)
        {
            // Calculate new size for zooming in
            Vector2 currentSize = uiRectTransform.sizeDelta;
            Vector2 newSize = currentSize * (1 + zoomSpeed);

            // Clamp size to be within min and max zoom levels
            newSize.x = Mathf.Clamp(newSize.x, minZoom, maxZoom);
            newSize.y = Mathf.Clamp(newSize.y, minZoom, maxZoom);

            // Apply the new size to the RectTransform
            if (newSize != currentSize)
            {
                uiRectTransform.sizeDelta = newSize;
            }
        }
    }

    private void ZoomOut()
    {
        if (uiRectTransform != null)
        {
            // Calculate new size for zooming out
            Vector2 currentSize = uiRectTransform.sizeDelta;
            Vector2 newSize = currentSize * (1 - zoomSpeed);

            // Ensure newSize does not go below minZoom
            newSize.x = Mathf.Max(newSize.x, minZoom);
            newSize.y = Mathf.Max(newSize.y, minZoom);

            // Clamp size to be within min and max zoom levels
            newSize.x = Mathf.Clamp(newSize.x, minZoom, maxZoom);
            newSize.y = Mathf.Clamp(newSize.y, minZoom, maxZoom);

            // Apply the new size to the RectTransform
            if (newSize != currentSize)
            {
                uiRectTransform.sizeDelta = newSize;
            }
        }
    } 
    public void ResetToDefault()
    {
        if (uiRectTransform != null)
        {
            // Reset position and size to default values
            uiRectTransform.anchoredPosition = defaultPosition;
            uiRectTransform.sizeDelta = defaultSize;
        }
        else
        {
            Debug.LogError("uiRectTransform is not assigned!");
        }
    }
    public void ResetToEvery()
    {
        isFollowingMouse = false;
        if (uiRectTransform != null)
        {
            // Reset position and size to default values
            uiRectTransform.anchoredPosition = defaultPosition;
            uiRectTransform.sizeDelta = defaultSize;
        }
        else
        {
            Debug.LogError("uiRectTransform is not assigned!");
        }
    }
}