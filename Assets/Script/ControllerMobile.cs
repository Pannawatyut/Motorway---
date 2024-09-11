using UnityEngine;
using Photon.Pun;

[RequireComponent(typeof(Rigidbody), typeof(BoxCollider))]
public class ControllerMobile : MonoBehaviourPun
{
    [SerializeField] private Rigidbody _rigidbody;
    [SerializeField] private Animator _animator;
    [SerializeField] private Transform _cameraTransform;

    [SerializeField] private float _moveSpeed = 5f; // Default move speed
    [SerializeField] private float _rotationSpeed = 5f; // Default rotation speed

    private Joystick _joystick;

#if UNITY_ANDROID || UNITY_IOS
    private void Start()
    {
        // Find the joystick in the scene by name
        _joystick = GameObject.Find("Fixed Joystick").GetComponent<Joystick>();

        if (_joystick == null)
        {
            Debug.LogError("Joystick not found! Please ensure there is a Joystick in the scene named 'Fixed Joystick'.");
        }
    }

    private void Update()
    {
        if (photonView.IsMine && _joystick != null)
        {
            Vector3 cameraForward = _cameraTransform.forward;
            Vector3 cameraRight = _cameraTransform.right;
            cameraForward.y = 0;
            cameraRight.y = 0;
            cameraForward.Normalize();
            cameraRight.Normalize();

            // Create movement vector based on joystick input
            Vector3 moveDirection = cameraForward * _joystick.Vertical + cameraRight * _joystick.Horizontal;
            Vector3 movement = moveDirection * _moveSpeed;

            // Set the Rigidbody's velocity
            _rigidbody.velocity = new Vector3(movement.x, _rigidbody.velocity.y, movement.z);

            // Rotate character to face movement direction
            if (_joystick.Horizontal != 0 || _joystick.Vertical != 0)
            {
                Quaternion targetRotation = Quaternion.LookRotation(moveDirection);
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * _rotationSpeed);
            }

            // Update animator with speed
            float speed = movement.magnitude;
            _animator.SetFloat("Speed", speed);
        }
    }
#else
    private void Start()
    {
        // This is a non-Android platform, so disable this script
        enabled = false;
    }
#endif
}
