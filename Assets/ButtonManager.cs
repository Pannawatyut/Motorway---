using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class ButtonManager : MonoBehaviour
{


    public void Login()
    {
        SceneManager.LoadScene("CustomCaracterMen 1");
    }

    public void StartGame()
    {
        SceneManager.LoadScene("Login");
    }

    public void Register()
    {
        SceneManager.LoadScene("Register");
    }

    public void SceneForgetPassword()
    {
        SceneManager.LoadScene("Forget_Password");
    }

}