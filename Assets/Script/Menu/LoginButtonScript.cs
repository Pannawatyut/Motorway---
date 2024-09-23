using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class LoginButtonScript : MonoBehaviour
{
    public LoginManager _loginManager;
    public TMP_InputField _Email;
    public MaskedPasswordScript _Password;
    public GameObject _loadingOk;
    public GameObject _loadingFailed;
    public GameObject _loading;
    public TextMeshProUGUI _errorMessage;

    private void Update()
    {
        if (_loginManager == null)
        {
            _loginManager = FindAnyObjectByType<LoginManager>();
        }
    }

    public void LoginButton()
    {
        _loginManager.OnClickLoginButton();
    }
}
