using System;
using System.Collections.Generic;
using Proyecto26;
using UnityEngine;

//using Vuplex.WebView.Internal;
/// <summary>
/// Handles calls to the Google provider for authentication
/// </summary>
public class GoogleAuthenticator : MonoBehaviour
{
    private const string ClientId = "GOOGLE_CLIENT_ID";
    private const string ClientSecret = "GOOGLE_CLIENT_SECRET";

    // Use the full URL as the redirect URI
    private static readonly string RedirectUri = "GETDATA";

    private static readonly HttpCodeListener codeListener = new HttpCodeListener();

    /// <summary>
    /// Opens a webpage that prompts the user to sign in and copy the auth code
    /// </summary>
    ///

    //public Vuplex.WebView.CanvasWebViewPrefab _WebviewObj;

    async void Start()
    {
        // Get a reference to the WebViewPrefab.

        // Wait until the prefab's initialized before accessing its WebView property.
        /*await _WebviewObj.WaitUntilInitialized();

        _WebviewObj.WebView.UrlChanged += (sender, eventArgs) => {
            // Replace myapp:// with your custom scheme
            if (eventArgs.Url.StartsWith("GETDATA"))
            {
                Debug.Log(eventArgs.Url);
                // TODO: Handle the custom scheme (e.g. parse the OAuth token from eventArgs.Url).
            }
        };*/
    }

    public void GetAuthCode()
    {
        /*_WebviewObj.gameObject.SetActive(true);
        _WebviewObj.InitialUrl = $"https://accounts.google.com/o/oauth2/v2/auth?client_id={ClientId}&redirect_uri={RedirectUri}&response_type=code&scope=email";
        
        //Application.OpenURL();

        codeListener.StartListening(code =>
        {
            ExchangeAuthCodeWithIdToken(code, idToken =>
            {
                FirebaseAuthHandler.SingInWithToken(idToken, "google.com");
            });
            
            codeListener.StopListening();
        });*/

        LoginWithGoogle();
    }

    public void LoginWithGoogle()
    {
        // เรียกใช้ JavaScript function จาก Unity WebGL
        Application.ExternalEval("googleSignIn();");
    }

    // ฟังก์ชันนี้จะรับ token จาก JavaScript หลังจากล็อกอินสำเร็จ

    public TMPro.TextMeshProUGUI _idToken;
    public LoginManager _Login;

    [Serializable]
    public class UserData
    {
        public string email;
        public string googleID;
        public string platform;
    }

    private string GetPlatform()
    {
#if UNITY_WEBGL
        return "WEBGL";
#elif UNITY_ANDROID
        return "ANDROID";
#elif UNITY_IOS
        return "IOS";
#else
        return "UNKNOWN";
#endif
    }

    public void OnGoogleSignInSuccess(string idToken)
    {
        Debug.Log("Google Sign-In Success with token: " + idToken);
        //_idToken.text = idToken;

        UserData userData = JsonUtility.FromJson<UserData>(idToken);
        GetPlatform();
        StartCoroutine(
            _Login._GoogleLoginAPI(userData.email, userData.googleID, userData.platform)
        );
        // คุณสามารถใช้ idToken นี้เพื่อทำงานอื่นๆ ต่อ เช่น Firebase Authentication ใน Unity
    }

    // public void _TestGoogle()
    // {
    //     StartCoroutine(
    //         _Login._GoogleLoginAPI("panatthakorn.isd@gmail.com", "116505355693568297360")
    //     );
    // }

    /// <summary>
    /// Exchanges the Auth Code with the user's Id Token
    /// </summary>
    /// <param name="code"> Auth Code </param>
    /// <param name="callback"> What to do after this is successfully executed </param>
    public static void ExchangeAuthCodeWithIdToken(string code, Action<string> callback)
    {
        try
        {
            RestClient
                .Request(
                    new RequestHelper
                    {
                        Method = "POST",
                        Uri = "https://oauth2.googleapis.com/token",
                        Params = new Dictionary<string, string>
                        {
                            { "code", code },
                            { "client_id", ClientId },
                            { "client_secret", ClientSecret },
                            { "redirect_uri", RedirectUri },
                            { "grant_type", "authorization_code" },
                        },
                    }
                )
                .Then(response =>
                {
                    var data =
                        StringSerializationAPI.Deserialize(
                            typeof(GoogleIdTokenResponse),
                            response.Text
                        ) as GoogleIdTokenResponse;
                    callback(data.id_token);
                })
                .Catch(Debug.Log);
        }
        catch (Exception e)
        {
            Debug.Log(e);
        }
    }

    public void _GoogleLogout()
    {
        
    }
}
