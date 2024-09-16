using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResetMovementAndCamera : MonoBehaviour
{
     public ThirdPersonOrbitCamBasic cameraScript;
     public MoveBehaviour moveBehaviour;
    
    public void Reset()
    {
        cameraScript.horizontalAimingSpeed = 6;
        cameraScript.verticalAimingSpeed = 6;
        moveBehaviour.walkSpeed = 0.15f;

    }
}
