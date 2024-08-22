using Photon.Pun;
using UnityEngine;

public class CameraLook : MonoBehaviourPun
{
    private float XRotation;
    private float YRotation;

    [SerializeField] private Transform TargetObject; // General target object
    public Vector2 LockAxis;
    public float Sensitivity = 40f;
    public float MaxVerticalAngle = 60f; // Maximum vertical angle
    public float CameraDistance = 5f; // Distance of the camera from the target object
    public float CameraHeight = 2f; // Height adjustment of the camera relative to the target object
    public float SmoothSpeed = 10f; // Smoothness of the camera movement

    private Vector3 currentVelocity = Vector3.zero;

    private void Update()
    {
        if (photonView.IsMine)
        {
            // Read input and update rotations
            float XMove = LockAxis.x * Sensitivity * Time.deltaTime;
            float YMove = LockAxis.y * Sensitivity * Time.deltaTime;

            // Update YRotation for horizontal rotation
            YRotation += XMove;

            // Update XRotation for vertical rotation
            XRotation -= YMove;

            // Clamp XRotation to prevent flipping the camera
            XRotation = Mathf.Clamp(XRotation, -MaxVerticalAngle, MaxVerticalAngle);

            // Update camera position and orientation
            UpdateCameraPosition();
        }
    }

    private void UpdateCameraPosition()
    {
        // Create a rotation based on the X and Y axis input
        Quaternion cameraRotation = Quaternion.Euler(XRotation, YRotation, 0);

        // Calculate the offset to maintain a consistent distance
        Vector3 offset = new Vector3(0, 0, -CameraDistance);
        Vector3 rotatedOffset = cameraRotation * offset;

        // Apply height adjustment
        rotatedOffset += Vector3.up * CameraHeight;

        // Smoothly move the camera to the target position
        Vector3 targetPosition = TargetObject.position + rotatedOffset;
        transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref currentVelocity, 1f / SmoothSpeed);

        // Make the camera look at the target object
        transform.LookAt(TargetObject.position);
    }
}