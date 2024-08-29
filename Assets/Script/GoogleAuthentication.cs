// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;
// using TMPro;
// using UnityEngine.UI;
// using Google;
// using System.Threading.Tasks;
// using UnityEngine.Networking;

// public class GoogleAuthentication : MonoBehaviour
// {

//     private GoogleSignInConfiguration configuration;
//     public string webClientId = "819459652226-a9l99jtljkrse17mbmn2lgpcp0ktbcvo.apps.googleusercontent.com";



//     void Awake()
//     {
//         configuration = new GoogleSignInConfiguration
//         {
//             WebClientId = webClientId,
//             RequestIdToken = true,
//             UseGameSignIn = false,
//             RequestEmail = true
//         };
//     }

//     public void OnSignIn()
//     {
//         GoogleSignIn.Configuration = configuration;
//         GoogleSignIn.DefaultInstance.SignIn().ContinueWith(
//             OnAuthenticationFinished, TaskScheduler.Default);
//     }

//     internal void OnAuthenticationFinished(Task<GoogleSignInUser> task)
//     {
//         if (task.IsFaulted)
//         {
//             using (IEnumerator<System.Exception> enumerator =
//                 task.Exception.InnerExceptions.GetEnumerator())
//             {
//                 if (enumerator.MoveNext())
//                 {
//                     GoogleSignIn.SignInException error =
//                         (GoogleSignIn.SignInException)enumerator.Current;
//                     Debug.LogError("Got Error: " + error.Status + " " + error.Message);
//                 }
//                 else
//                 {
//                     Debug.LogError("Got unexpected exception?!?" + task.Exception);
//                 }
//             }
//         }
//         else if (task.IsCanceled)
//         {
//             Debug.LogError("Cancelled");
//         }

//     }

//     public void OnSignOut()
//     {

//         Debug.Log("Calling SignOut");
//         GoogleSignIn.DefaultInstance.SignOut();
//     }

// }