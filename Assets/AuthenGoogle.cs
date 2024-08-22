using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Google;
using TMPro;
using UnityEngine.UI; // Corrected from UnityEditor.UI to UnityEngine.UI
using System.Threading.Tasks;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;

public class AuthenGoogle : MonoBehaviour
{
    public string ImageURL;
    public TMP_Text usernameText, userEmailText;
    public Image profilePic;
    public string webClientId = "275388235191-b4pepee4rl54ctqu8i0596h122bpid22.apps.googleusercontent.com";
    public GameObject loginPanel, profilePanel;
    private GoogleSignInConfiguration configuration;

    void Awake() {
        configuration = new GoogleSignInConfiguration {
            WebClientId = webClientId,
            RequestIdToken = true
        };
    }

    public void OnSignIn() {
        GoogleSignIn.Configuration = configuration;
        GoogleSignIn.Configuration.UseGameSignIn = false;
        GoogleSignIn.Configuration.RequestIdToken = true;
        GoogleSignIn.Configuration.RequestEmail = true; // Corrected typo here
        GoogleSignIn.DefaultInstance.SignIn().ContinueWith(
            OnAuthenticationFinished, TaskScheduler.FromCurrentSynchronizationContext());
    }

    internal void OnAuthenticationFinished(Task<GoogleSignInUser> task) {
        if (task.IsFaulted) {
            using (IEnumerator<System.Exception> enumerator = task.Exception.InnerExceptions.GetEnumerator()) {
                if (enumerator.MoveNext()) {
                    GoogleSignIn.SignInException error = (GoogleSignIn.SignInException)enumerator.Current;
                    Debug.LogError("Got Error: " + error.Status + " " + error.Message);
                } else {
                    Debug.LogError("Got Unexpected Exception?!?" + task.Exception);
                }
            }
        } else if (task.IsCanceled) {
            Debug.LogError("Canceled");
        } else {
            Debug.Log("Welcome: " + task.Result.DisplayName + "!");
            // usernameText.text = task.Result.DisplayName;
            // userEmailText.text = task.Result.Email;
            // ImageURL = task.Result.ImageUrl.ToString();

            // loginPanel.SetActive(false);
            // profilePanel.SetActive(true);

            // StartCoroutine(LoadProfilePic());

            // Load the Login scene
            SceneManager.LoadScene("Register");
        }
    }

    IEnumerator LoadProfilePic() {
        UnityWebRequest www = UnityWebRequestTexture.GetTexture(ImageURL);
        yield return www.SendWebRequest();

        if (www.result != UnityWebRequest.Result.Success) {
            Debug.LogError("Error Loading Profile Pic: " + www.error);
        } else {
            Texture2D texture = ((DownloadHandlerTexture)www.downloadHandler).texture;
            profilePic.sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0, 0));
        }
    }

    public void OnSignOut() {
        usernameText.text = "";
        userEmailText.text = "";
        ImageURL = "";

        loginPanel.SetActive(true);
        profilePanel.SetActive(false);
        Debug.Log("Calling SignOut");
        GoogleSignIn.DefaultInstance.SignOut();
    }
}
