using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelecTap : MonoBehaviour
{
    public GameObject[] button;
    public GameObject[] collecItem;
    public Animator animator;

    public GameObject[] objectRotate;
    public float rotateSpeed = 50f;
    bool rotateStatus = false;

    public float rotationSpeed = 10f;  
    private bool isRotating = false;    
    private float lastMouseX;


    public void OnCategoryClick(int categoryIndex)
    {
        for (int i = 0; i < button.Length; i++)
        {
            button[i].SetActive(i == categoryIndex);
            collecItem[i].SetActive(i == categoryIndex);


        }
        switch (categoryIndex)
        {
            case 0:
                animator.Play("Hair"); 
                break;
            case 1:
                animator.Play("Face"); 
                break;
            case 2:
                animator.Play("Shirts"); 
                break;
            case 3:
                animator.Play("Pants");
                break;
            case 4:
                animator.Play("Shoes");
                break;
            case 5:
                animator.Play("Accessory");
                break;
        }
    }
    public void RotateObject()
    {

        if (rotateStatus == false)
        {
            rotateStatus = true;
        }
        else
        {
            rotateStatus = false;
        }
    }

    void Update()
    {
        if (rotateStatus == true)
        {
            //rotate object with speed
            objectRotate[0].transform.Rotate(Vector3.up, rotateSpeed * Time.deltaTime);
            objectRotate[1].transform.Rotate(Vector3.up, rotateSpeed * Time.deltaTime);
        }
                // ตรวจจับว่ามีการกดปุ่มเมาส์ข้างซ้าย (หรือค้างเมาส์)
        if (Input.GetMouseButtonDown(0))
        {
            isRotating = true;
            lastMouseX = Input.mousePosition.x;
        }
        else if (Input.GetMouseButtonUp(0))
        {
            isRotating = false;
        }

        // ถ้ากำลังหมุนอยู่
        if (isRotating && objectRotate != null)
        {
            float currentMouseX = Input.mousePosition.x;
            float mouseDeltaX = currentMouseX - lastMouseX;

            // หมุนทุก GameObject ในอาร์เรย์ objectRotate ในแกน Y
            foreach (GameObject obj in objectRotate)
            {
                if (obj != null)
                {
                    obj.transform.Rotate(Vector3.up, -mouseDeltaX * rotationSpeed * Time.deltaTime);
                }
            }

            lastMouseX = currentMouseX;
        }
    }

}
