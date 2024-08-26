using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playanimation : MonoBehaviour
{
    private Vector3 lastPosition;
    private Animator animator;

    void Start()
    {
        // บันทึกตำแหน่งเริ่มต้นของวัตถุ
        lastPosition = transform.position;

        // รับ Animator Component ที่แนบมากับวัตถุ
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        // ตรวจสอบการเปลี่ยนแปลงของตำแหน่ง
        if (transform.position != lastPosition)
        {
            // วัตถุเคลื่อนที่
            animator.Play("Wheel");

            // อัพเดทตำแหน่งล่าสุด
            lastPosition = transform.position;
        }
    }
}