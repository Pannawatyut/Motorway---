using UnityEngine;

public class UILookAt : MonoBehaviour
{
    public Camera targetCamera; 
    public Transform playerTransform; 

    private void Start()
    {
        // ถ้า targetCamera เป็น null ให้ใช้ Camera.main
        if (targetCamera == null)
        {
            targetCamera = Camera.main;
        }
    }

    private void LateUpdate()
    {
        // ตรวจสอบว่ามี targetCamera และ playerTransform ที่กำหนดหรือไม่
        if (targetCamera == null)
        {
            Debug.LogWarning("No target camera assigned or found.");
            return;
        }

        if (playerTransform == null)
        {
            Debug.LogWarning("No player transform assigned.");
            return;
        }

        // อัปเดตตำแหน่งของ UI ให้อยู่ที่ตำแหน่งของ Player
        transform.position = playerTransform.position;

        // คำนวณทิศทางจาก UI ไปยังกล้อง
        Vector3 directionToCamera = targetCamera.transform.position - transform.position;

        // คำนวณการหมุนที่ทำให้ UI มองไปทางกล้อง
        Quaternion lookRotation = Quaternion.LookRotation(-directionToCamera, Vector3.up);

        // ตั้งค่าการหมุนของ UI โดยไม่เปลี่ยนตำแหน่ง
        transform.rotation = lookRotation;
    }
}