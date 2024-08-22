using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections.Generic;

public class CharacterCountDisplay : MonoBehaviour
{
    public InputField inputField;
    public TextMeshProUGUI characterCountText;
    public GameObject feedbackText;
    public GameObject Nulltext;

    // เพิ่มคำหยาบที่ต้องการบล็อค พร้อมรูปแบบ Leet ที่เป็นไปได้
    public List<string> profaneWords = new List<string> 
    { 
        // คำหยาบภาษาไทย
        "ไอ้", "อี", "เหี้ย", "เชี่ย", "แม่ง", "สัส", 
        "สัด", "มึง", "กู", "ห่า", "ควย", 
        "เย็ด", "หี", "แตด", "ฟาย", "ตอแหล", "ระยำ", 
        "สัตว์", "ดอกทอง", "จัญไร", "ชาติชั่ว", "เห้", 
        "พ่อง", "แม่ม", "สถุน", "กาก", "สวะ", "หน้าตัวเมีย",

        // คำหยาบภาษาอังกฤษ
        "fuck", "shit", "bitch", "asshole", "bastard", "dick", 
        "pussy", "cunt", "motherfucker", "nigger", "fag", 
        "slut", "whore", "douche", "cock", "jerk", "freak", 
        "suck", "crap", "dumbass", "retard", "idiot", "moron",

        // รูปแบบ Leet ภาษาไทย
        "คuย", "ค_ย", "ควe", "ค.วย",
        "hี", "ฮี",
        "เ_้ย", "เ_ี้ย", "เ-้ย", "เ-ี้ย",
        "สaส", "สั_ส", "สัต", "สัตว์",
        "เ_ด", "เ-ด", "เ_ย็ด", "เย็d",

        // รูปแบบ Leet ภาษาอังกฤษ
        "f@ck", "sh!t", "b!tch", "4ss", "b@stard", "d!ck", 
        "p@ssy", "c*nt", "m0therfucker", "n!gger", "f@g", 
        "sl*t", "wh0re", "d0uche", "c*ck", "j3rk", "fr3ak", 
        "suck", "cr@p", "dumb@ss", "r3tard", "idi0t", "m0r0n"
    }; 

    void Start()
    {
        if (inputField != null)
        {
            inputField.onValueChanged.AddListener(UpdateCharacterCountAndFilterProfanity);
            UpdateCharacterCountAndFilterProfanity(inputField.text); // Initial update
        }
    }

    void UpdateCharacterCountAndFilterProfanity(string text)
    {
        if (inputField != null && characterCountText != null)
        {
            // ตรวจสอบคำหยาบ
            bool hasProfanity = ContainsProfanity(text);
            if (hasProfanity)
            {
                feedbackText.SetActive(true);
                Nulltext.SetActive(false);
                // Optionally, you can clear the input field if it contains profanity
                // inputField.text = ""; // Uncomment this line if you want to clear the text
            }
            else
            {
                feedbackText.SetActive(false);
            }

            // อัปเดตจำนวนตัวอักษร
            int currentTextLength = inputField.text.Length;
            int characterLimit = inputField.characterLimit;
            characterCountText.text = currentTextLength + "/" + characterLimit;
        }
    }

    bool ContainsProfanity(string text)
    {
        text = text.ToLower(); // แปลงข้อความให้เป็นตัวพิมพ์เล็กทั้งหมดเพื่อตรวจสอบอย่างไม่สนใจ case

        foreach (string profaneWord in profaneWords)
        {
            if (text.Contains(profaneWord.ToLower())) // ตรวจสอบคำหยาบพร้อมรูปแบบ Leet
            {
                return true;
            }
        }
        return false;
    }
}