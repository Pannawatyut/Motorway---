// using UnityEngine;
// using System.Runtime.InteropServices;

// public class GoogleAuth : MonoBehaviour
// {
//     [DllImport("__Internal")]
//     private static extern void initGoogleSignIn(string clientId);

//     void Start()
//     {
//         string clientId = "819459652226-a9l99jtljkrse17mbmn2lgpcp0ktbcvo.apps.googleusercontent.com";
//         initGoogleSignIn(clientId);
//     }

//     public void OnGoogleSignInSuccess(string idToken)
//     {
//         Debug.Log("Google Sign-In successful, ID Token: " + idToken);
//     }
// }
