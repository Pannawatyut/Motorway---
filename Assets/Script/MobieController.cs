using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody), typeof(BoxCollider))]
public class MobieController : MonoBehaviour
{
    [SerializeField] private Rigidbody _rigidbody;
    [SerializeField] private FixedJoystick _joystick;
    [SerializeField] private Animator _animator;

    [SerializeField] private float _moveSpeed;

    private void FixedUpdate()
    {
        // คำนวณความเร็วของ Rigidbody ตาม Joystick
        Vector3 movement = new Vector3(_joystick.Horizontal * _moveSpeed, 0f, _joystick.Vertical * _moveSpeed);

        // กำหนดความเร็วของ Rigidbody เพื่อเคลื่อนที่ในแนว X และ Z เท่านั้น
        _rigidbody.velocity = new Vector3(movement.x, _rigidbody.velocity.y, movement.z);

        // ควบคุมการหมุนของตัวละคร
        if (_joystick.Horizontal != 0 || _joystick.Vertical != 0)
        {
            transform.rotation = Quaternion.LookRotation(movement);
        }

        // อัปเดตพารามิเตอร์ Speed ใน Animator
        float speed = movement.magnitude; // ความเร็วเป็น magnitude ของเวกเตอร์
        _animator.SetFloat("Speed", speed);
    }
}