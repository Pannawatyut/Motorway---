using UnityEngine;

public class CameraControl : MonoBehaviour
{
    public bool isLocked = false;
    public float rotationSpeed = 5.0f;
    public float movementSpeed = 5.0f;

    private void Update()
    {
        if (isLocked) return; // Skip camera controls if locked

        // Example camera movement
        float horizontal = Input.GetAxis("Mouse X") * rotationSpeed;
        float vertical = Input.GetAxis("Mouse Y") * rotationSpeed;

        transform.Rotate(Vector3.up, horizontal);
        transform.Rotate(Vector3.left, vertical);
        
        // Example camera movement logic (not complete)
        float move = Input.GetAxis("Vertical") * movementSpeed * Time.deltaTime;
        transform.Translate(Vector3.forward * move);
    }
}