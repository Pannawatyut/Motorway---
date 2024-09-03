using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class entername : MonoBehaviour
{
    public GameObject[] Button;
    public InputField username;
    public TextMeshProUGUI output;
    public TextMeshProUGUI UserRequir;
    public GameObject feedbackText;
    public LoginManager _loginManager;
    // List of forbidden words
    public List<string> profaneWords = new List<string>();

    private void Start()
    {
        // Populate the list with default forbidden words
        #region PandoraBox
        profaneWords = new List<string>
        {
         "fuck", "fck", "f4ck", "f@ck", "f u c k", "f_ck", "fucking", "fuxk",
         "bitch", "b1tch", "bi7ch", "biatch", "sh1t", "sht", "sh!t", "asshole",
         "a$$hole", "a$$", "a s s", "dick", "d1ck", "d!ck", "pussy", "pssy",
         "p@ssy", "cunt", "cnt", "c@nt", "sex", "s3x", "s e x", "fook", "fucc",
         "fakk", "ควย", "คย", "คอ-วย", "ควe", "คว@", "ค ว ย", "คว€ย", "ควาe",
         "ควาย", "ค ว า ย", "หี", "ฮี", "hี", "หe", "he", "hี", "he3", "hee",
         "เฮี้ย", "เหี้ย", "เย็ด", "yd", "เยd", "เยeด", "เยด", "เย็ด", "เยด",
         "เย็xด", "เxด", "เยD", "เยDด", "เ ย็ ด", "ดอกทอง", "ดอกทoง", "ดอกทอง",
         "โดกทง", "อีดอก", "อีดoก", "อีดอk", "ส้นตีน", "ส้นตีu", "ส้นต่ีน",
         "ส ตี น", "ส้น ติ น", "ตีน", "ตีu", "kuy", "hee", "ted", "yed",
         "ky", "he", "td", "yd", "kuy", "h33", "t3d", "y3d",
         "เซ็ก", "เซ็กส์", "เซ็กซี่", "เซ๊ก", "เซ่ก",
         "S3x", "S3xx", "S3ks", "YesHEE", "Y3sHEE", "Y@SHEE",
         "HEE", "H33", "H3E", "T3D", "T@D", "T£D", "YED", "Y3D", "Y£D",
         "KUI", "K¥I", "KUI", "Y3T", "YET", "HEE!", "H3E!", "HEE@","วชิรา","HE3","HE3!","ดoก","ดอกทง",
         "ดอกทj","แตด","llตด","llตd","เกลียด","นัดเยส","สวะ","เxี้ย","ไอ้","อี","เต๋า","กะหรี่","KUY","Kuy","kUy","kuY","kuy","KuY","KUy",
         "kUy","kUY","Kuuy","KUuy","kUUy","KuUy","CuM","cUm","CUm","cum","cuM","KUUY","KUU","kuu","Kuu","Dick","Pussy","DICK","dick",
         "kUU","PUSSY","pussy","Sawa","SAWA","sawa"

        };
        #endregion
    }

    // Start is called before the first frame update
    public void Open()
    {
        output.text = "";
        Button[0].SetActive(true);
    }

    public void Off()
    {
        Button[0].SetActive(false);
        Button[1].SetActive(false);
        UserRequir.gameObject.SetActive(false);
        feedbackText.SetActive(false);
    }

    private void Update()
    {
        if (_loginManager == null)
        {
            _loginManager = FindObjectOfType<LoginManager>();
        }

        if (_loginManager._Avatar.name != null)
        {
            this.gameObject.SetActive(false);
        }
    }
    public void NameCheck()
    {
        string usernameText = username.text;
        if (ContainsProfanity(usernameText))
        {
            Debug.Log("Username contains forbidden words.");
            feedbackText.SetActive(true); // Show the warning about forbidden words
            return;
        }
    }
    // This method will be called when the user tries to proceed to the next step
    public void Next()
    {
        string usernameText = username.text;

        if (string.IsNullOrEmpty(usernameText))
        {
            Debug.Log("Username is required.");
            UserRequir.gameObject.SetActive(true);
            return;
        }

        if (ContainsProfanity(usernameText))
        {
            Debug.Log("Username contains forbidden words.");
            feedbackText.SetActive(true); // Show the warning about forbidden words
            return;
        }

        output.text = usernameText;
        Button[1].SetActive(true);
        UserRequir.gameObject.SetActive(false);
        feedbackText.SetActive(false);
    }

    // Function to check if the input contains any forbidden words
    bool ContainsProfanity(string text)
    {
        foreach (string profaneWord in profaneWords)
        {
            
            if (text.Contains(profaneWord))
            {
                return true;
            }
        }
        return false;
    }

  
}
