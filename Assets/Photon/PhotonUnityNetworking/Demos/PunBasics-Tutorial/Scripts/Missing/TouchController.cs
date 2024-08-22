using UnityEngine;

public class TouchController : MonoBehaviour
{
    public FixedTouchField _FixedTouchField;
    public CameraLook _CameraLook;
    
    void Update()
    {
        if (_CameraLook == null)
        {
            // Search for the CameraLook component on CameraPlayer if not already assigned
            GameObject cameraPlayer = GameObject.Find("CameraPlayer");
            if (cameraPlayer != null)
            {
                _CameraLook = cameraPlayer.GetComponent<CameraLook>();
                if (_CameraLook == null)
                {
                    Debug.LogWarning("CameraLook component not found on CameraPlayer!");
                }
            }
            else
            {
                Debug.LogWarning("CameraPlayer object not found in the scene!");
            }
        }

        if (_CameraLook != null)
        {
            // Update LockAxis in CameraLook with the touch delta
            _CameraLook.LockAxis = _FixedTouchField.TouchDist;
        }
    }
}
