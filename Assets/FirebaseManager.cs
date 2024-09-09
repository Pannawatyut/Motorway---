using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Firebase;
using Firebase.Extensions;
using Google;
using UnityEngine;
using UnityEngine.UI;

public class FirebaseManager : MonoBehaviour
{
    private string GoogleAPI =
        "244382866689-oh99255jkhtrebe6f2qk0rehhab0kgp5.apps.googleusercontent.com";
    public LoginManager _loginManager;
    private GoogleSignInConfiguration configuration;

    Firebase.DependencyStatus dependencyStatus = Firebase.DependencyStatus.UnavailableOther;
    Firebase.Auth.FirebaseAuth auth;
    Firebase.Auth.FirebaseUser user;

    public Text Username,
        UserEmail;

    public GameObject LoginScreen,
        ProfileScreen;

    private void Awake()
    {
        configuration = new GoogleSignInConfiguration
        {
            WebClientId = GoogleAPI,
            RequestIdToken = true,
            RequestEmail = true,
        };
    }

    private void Start()
    {
        InitFirebase();
    }

    void InitFirebase()
    {
        FirebaseApp
            .CheckAndFixDependenciesAsync()
            .ContinueWithOnMainThread(task =>
            {
                dependencyStatus = task.Result;
                if (dependencyStatus == Firebase.DependencyStatus.Available)
                {
                    auth = Firebase.Auth.FirebaseAuth.DefaultInstance;
                }
                else
                {
                    Debug.LogError(
                        "Could not resolve all Firebase dependencies: " + dependencyStatus
                    );
                }
            });
    }

    public void GoogleSignInClick()
    {
        GoogleSignIn.Configuration = configuration;
        GoogleSignIn.Configuration.UseGameSignIn = false;
        GoogleSignIn.Configuration.RequestIdToken = true;
        GoogleSignIn.Configuration.RequestEmail = true;

        GoogleSignIn.DefaultInstance.SignIn().ContinueWith(OnGoogleAuthenticatedFinished);
    }

    void OnGoogleAuthenticatedFinished(Task<GoogleSignInUser> task)
    {
        if (task.IsFaulted)
        {
            foreach (var exception in task.Exception.InnerExceptions)
            {
                GoogleSignIn.SignInException signInException =
                    exception as GoogleSignIn.SignInException;
                if (signInException != null)
                {
                    Debug.LogError("Google Sign-In failed with status: " + signInException.Status);
                }
                else
                {
                    Debug.LogError("Google Sign-In failed: " + exception.ToString());
                }
            }
        }
        else if (task.IsCanceled)
        {
            Debug.LogError("Google Sign-In was canceled.");
        }
        else
        {
            Firebase.Auth.Credential credential = Firebase.Auth.GoogleAuthProvider.GetCredential(
                task.Result.IdToken,
                null
            );

            auth.SignInWithCredentialAsync(credential)
                .ContinueWithOnMainThread(authTask =>
                {
                    if (authTask.IsCanceled)
                    {
                        Debug.LogError("SignInWithCredentialAsync was canceled.");
                        return;
                    }

                    if (authTask.IsFaulted)
                    {
                        Debug.LogError(
                            "SignInWithCredentialAsync encountered an error: " + authTask.Exception
                        );
                        return;
                    }

                    user = auth.CurrentUser;

                    Username.text = user.DisplayName;
                    UserEmail.text = user.Email;

                    LoginScreen.SetActive(false);
                    ProfileScreen.SetActive(true);

                    // Send email and Google ID to the backend
                    StartCoroutine(_loginManager._GoogleLoginAPI(user.Email, user.UserId));
                });
        }
    }

    // private string CheckImageUrl(string url) {
    //     if (!string.IsNullOrEmpty(url)) {
    //         return url;
    //     }
    //     return imageUrl;
    // }

    // IEnumerator LoadImage(string imageUri) {
    //     WWW www = new WWW(imageUri);
    //     yield return www;

    //     UserProfilePic.sprite = Sprite.Create(www.texture, new Rect(0, 0, www.texture.width, www.texture.height), new Vector2(0, 0));
    // }
}
