using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playanimation : MonoBehaviour
{
    private Vector3 lastPosition;
    private Animator animator;
    private MinigameAudioScript _AudioScript;
    public int CarIndex;

    void Start()
    {
        // บันทึกตำแหน่งเริ่มต้นของวัตถุ
        lastPosition = transform.position;

        // รับ Animator Component ที่แนบมากับวัตถุ
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        if (_AudioScript == null)
        {
            _AudioScript = FindAnyObjectByType<MinigameAudioScript>();
        }
        // ตรวจสอบการเปลี่ยนแปลงของตำแหน่ง
        if (transform.position != lastPosition)
        {
            // วัตถุเคลื่อนที่
            animator.Play("Wheel");
            
            // อัพเดทตำแหน่งล่าสุด
            lastPosition = transform.position;
            //if (CarIndex == 0)
            //{
            //    _AudioScript._SmallCarSound.Play();
            //}
            //else if (CarIndex == 1)
            //{
            //    _AudioScript._MediumCarSound.Play();
            //}
            //else if (CarIndex == 2)
            //{
            //    _AudioScript._LargeCarSound.Play();
            //}
        }
    }
}