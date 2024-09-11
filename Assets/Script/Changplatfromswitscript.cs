using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Changplatfromswitscript : MonoBehaviour
{
    public MonoBehaviour[] ScriptChange;

    void Start()
    {
#if UNITY_ANDROID || UNITY_IOS
        // เปิดใช้งานสคริปต์ที่อยู่ใน ScriptChange[1] และปิด ScriptChange[0]
        ScriptChange[1].enabled = true;
        ScriptChange[0].enabled = false;
#else
        // เปิดใช้งานสคริปต์ที่อยู่ใน ScriptChange[0] และปิด ScriptChange[1]
        ScriptChange[0].enabled = true;
        ScriptChange[1].enabled = false;
#endif
    }
}
