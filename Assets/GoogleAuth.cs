#if UNITY_ANDROID || UNITY_IOS || UNITY_STANDALONE
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Firebase;
using Firebase.Auth;
using Google;
using UnityEngine;
using UnityEngine.UI;
#endif

#if UNITY_WEBGL
using FirebaseWebGL.Examples.Utils;
using FirebaseWebGL.Scripts.FirebaseBridge;
using FirebaseWebGL.Scripts.Objects;
using TMPro;
using UnityEngine;
#endif

namespace FirebaseWebGL.Examples.Auth
{
    public class GoogleAuth : MonoBehaviour
    {

        public GameObject[] _BTM;
        public void OnEnable()
        {
            /*_BTM[0].SetActive(false);
            _BTM[1].SetActive(false);

            if (UserData.instance._UserType == UserData._Type.PC)
            {
                _BTM[0].SetActive(true);
                _BTM[1].SetActive(false);
            }
            else
            {
                _BTM[0].SetActive(false);
                _BTM[1].SetActive(true);
            }*/
        }

        public string webClientId = "";

#if UNITY_ANDROID || UNITY_IOS
        //public string webClientId = "";

        private FirebaseAuth auth;
        private GoogleSignInConfiguration configuration;

        private void Awake()
        {
            configuration = new GoogleSignInConfiguration { WebClientId = webClientId, RequestEmail = true, RequestIdToken = true };
            Debug.Log(webClientId);
            CheckFirebaseDependencies();
        }

        private void CheckFirebaseDependencies()
        {
            FirebaseApp.CheckAndFixDependenciesAsync().ContinueWith(task =>
            {
                if (task.IsCompleted)
                {
                    if (task.Result == DependencyStatus.Available)
                        auth = FirebaseAuth.DefaultInstance;
                    else
                    //AddToInformation("Could not resolve all Firebase dependencies: " + task.Result.ToString());
                    Debug.Log("Could not resolve all Firebase dependencies: " + task.Result.ToString());
                }
                else
                {
                //AddToInformation("Dependency check was not completed. Error : " + task.Exception.Message);
            }
            });
        }

        public void SignInWithGoogle() {

            OnSignIn(); 

        }
        public void SignOutFromGoogle() { OnSignOut(); }

        private void OnSignIn()
        {
            //infoText.text = "";
            GoogleSignIn.Configuration = configuration;
            GoogleSignIn.Configuration.UseGameSignIn = false;
            GoogleSignIn.Configuration.RequestIdToken = true;
            //AddToInformation("Calling SignIn");
            Debug.Log("Calling SignIn");

            GoogleSignIn.DefaultInstance.SignIn().ContinueWith(OnAuthenticationFinished);
        }

        private void OnSignOut()
        {
            //infoText.text = "";
            //AddToInformation("Calling SignOut");
            Debug.Log("Calling SignOut");
            GoogleSignIn.DefaultInstance.SignOut();
        }

        public void OnDisconnect()
        {
            //infoText.text = "";
            //AddToInformation("Calling Disconnect");
            Debug.Log("Calling Disconnect");
            GoogleSignIn.DefaultInstance.Disconnect();
        }

        internal void OnAuthenticationFinished(Task<GoogleSignInUser> task)
        {
            if (task.IsFaulted)
            {
                using (IEnumerator<Exception> enumerator = task.Exception.InnerExceptions.GetEnumerator())
                {
                    if (enumerator.MoveNext())
                    {
                        GoogleSignIn.SignInException error = (GoogleSignIn.SignInException)enumerator.Current;
                        //AddToInformation("Got Error: " + error.Status + " " + error.Message);
                        //AddToInformation("Got Error: " + error);
                        Debug.Log("Got Error: " + error.Status + " " + error.Message);
                        Debug.Log("Got Error: " + error);
                    }
                    else
                    {
                        //AddToInformation("Got Unexpected Exception?!?" + task.Exception);
                        Debug.Log("Got Unexpected Exception?!?" + task.Exception);
                    }
                }
            }
            else if (task.IsCanceled)
            {
                //AddToInformation("Canceled");
                Debug.Log("Canceled");
            }
            else
            {
                //AddToInformation("Welcome: " + task.Result.DisplayName + "!");
                //AddToInformation("Email = " + task.Result.Email);
                //AddToInformation("Google ID Token = " + task.Result.IdToken);
                //AddToInformation("Email = " + task.Result.Email);
                Debug.Log("Welcome: " + task.Result.DisplayName + "!");
                Debug.Log("Email = " + task.Result.Email);
                Debug.Log("Google ID Token = " + task.Result.IdToken);
                Debug.Log("Email = " + task.Result.Email);
                UserData.instance._CallBackEmail = task.Result.Email;
                // _LoginManager._CheckThirdParty();
                StartCoroutine(_LoginManager._GoogleLoginAPI(UserData.instance._CallBackEmail, task.Result.IdToken));
                SignInWithGoogleOnFirebase(task.Result.IdToken);
            }
        }

        public LoginManager _LoginManager;

        private void SignInWithGoogleOnFirebase(string idToken)
        {
            Credential credential = GoogleAuthProvider.GetCredential(idToken, null);

            auth.SignInWithCredentialAsync(credential).ContinueWith(task =>
            {
                AggregateException ex = task.Exception;
                if (ex != null)
                {
                    if (ex.InnerExceptions[0] is FirebaseException inner && (inner.ErrorCode != 0))
                    //AddToInformation("\nError code = " + inner.ErrorCode + " Message = " + inner.Message);
                    Debug.Log("\nError code = " + inner.ErrorCode + " Message = " + inner.Message);
                }
                else
                {
                //AddToInformation("Sign In Successful.");
                Debug.Log("Sign In Successful.");
                }
            });
        }

        public void OnSignInSilently()
        {
            GoogleSignIn.Configuration = configuration;
            GoogleSignIn.Configuration.UseGameSignIn = false;
            GoogleSignIn.Configuration.RequestIdToken = true;
            //AddToInformation("Calling SignIn Silently");

            GoogleSignIn.DefaultInstance.SignInSilently().ContinueWith(OnAuthenticationFinished);
        }

        public void OnGamesSignIn()
        {
            GoogleSignIn.Configuration = configuration;
            GoogleSignIn.Configuration.UseGameSignIn = true;
            GoogleSignIn.Configuration.RequestIdToken = false;

            //AddToInformation("Calling Games SignIn");

            GoogleSignIn.DefaultInstance.SignIn().ContinueWith(OnAuthenticationFinished);

            
        }

#endif

#if UNITY_WEBGL
public TMP_InputField emailInputField;
        public TMP_InputField passwordInputField;

        public TextMeshProUGUI outputText;

        public string _Email;
        public string uid;

        private void Start()
        {
            if (Application.platform != RuntimePlatform.WebGLPlayer)
            {
                DisplayError("The code is not running on a WebGL build; as such, the Javascript functions will not be recognized.");
                return;
            }
            
            FirebaseAuth.OnAuthStateChanged(gameObject.name, "DisplayUserInfo", "DisplayInfo");

        }

        public void CreateUserWithEmailAndPassword() =>
            FirebaseAuth.CreateUserWithEmailAndPassword(emailInputField.text, passwordInputField.text, gameObject.name, "DisplayInfo", "DisplayErrorObject");

        public void SignInWithEmailAndPassword() =>
            FirebaseAuth.SignInWithEmailAndPassword(emailInputField.text, passwordInputField.text, gameObject.name, "DisplayInfo", "DisplayErrorObject");
        
        public void SignInWithGoogle() =>
            FirebaseAuth.SignInWithGoogle(gameObject.name, "DisplayInfo", "DisplayErrorObject");
        
        public void SignInWithFacebook() =>
            FirebaseAuth.SignInWithFacebook(gameObject.name, "DisplayInfo", "DisplayErrorObject");
        
        public void DisplayUserInfo(string user)
        {
            var parsedUser = StringSerializationAPI.Deserialize(typeof(FirebaseUser), user) as FirebaseUser;
            DisplayData($"Email: {parsedUser.email}, UserId: {parsedUser.uid}, EmailVerified: {parsedUser.isEmailVerified}");
            _Email = parsedUser.email;
            uid = parsedUser.uid;
            StartCoroutine(_LoginManager._GoogleLoginAPI(_Email, uid));
            //_LoginManager._CheckThirdParty();
        }
        public LoginManager _LoginManager;
        public void DisplayData(string data)
        {
            Debug.Log("A");
            //var parsedUser = StringSerializationAPI.Deserialize(typeof(FirebaseUser), data) as FirebaseUser;
            //ebug.Log("A : " + parsedUser.email +" / "+parsedUser.uid);
           // _Email = parsedUser.email;
            //uid = parsedUser.uid;
            //StartCoroutine(_LoginManager._GoogleLoginAPI(_Email, uid));
            //outputText.color = outputText.color == Color.green ? Color.blue : Color.green;
            //outputText.text = data;
            Debug.Log(data);
        }

        public void DisplayInfo(string info)
        {
            Debug.Log("B");
            //var parsedUser = StringSerializationAPI.Deserialize(typeof(FirebaseUser), info) as FirebaseUser;
           // Debug.Log("B : " + parsedUser.email + " / " + parsedUser.uid);
            //_Email = parsedUser.email;
            //uid = parsedUser.uid;
            //StartCoroutine(_LoginManager._GoogleLoginAPI(_Email, uid));
            //outputText.color = Color.white;
            //outputText.text = info;
            Debug.Log(info);
        }

        public void DisplayErrorObject(string error)
        {
            var parsedError = StringSerializationAPI.Deserialize(typeof(FirebaseError), error) as FirebaseError;
            DisplayError(parsedError.message);

            Debug.Log("C");
        }

        public void DisplayError(string error)
        {
            //outputText.color = Color.red;
            //outputText.text = error;
            Debug.LogError(error);

            Debug.Log("D");

           
        }

        public void _TestGoogleSignOn()
        {
            StartCoroutine(_LoginManager._GoogleLoginAPI("tammarat.amp.metaversexr@gmail.com", "uw2TzOs3ZeXkNuiFsgy39XDZkrJY2"));
        }
#endif
    }
}
