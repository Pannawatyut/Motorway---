using UnityEngine;
using UnityEngine.SceneManagement;
using Photon.Realtime;
using Photon.Pun;
using Unity.VisualScripting;

namespace Photon.Pun.Demo.PunBasics
{
    public class GameManager : MonoBehaviourPunCallbacks
    {
        public static GameManager Instance;

        private GameObject instance;

        public LoginManager _LoginManager;

        [Tooltip("The male prefab to use for representing the player")]
        [SerializeField]
        private GameObject malePlayerPrefab;

        [Tooltip("The female prefab to use for representing the player")]
        [SerializeField]
        private GameObject femalePlayerPrefab;

        void Start()
        {
            Instance = this;

            if (_LoginManager == null)
            {
                _LoginManager = FindObjectOfType<LoginManager>();
            }

            PhotonNetwork.ConnectUsingSettings();

            
        }

        public override void OnConnectedToMaster()
        {
                PhotonNetwork.JoinRandomOrCreateRoom();
        }

        public override void OnJoinedRoom()
        {
            if (PlayerManager.LocalPlayerInstance == null)
            {
                Debug.Log("Trying to spawn Player");
                InstantiatePlayer();
            }
        }

        public override void OnJoinRandomFailed(short returnCode, string message)
        {
            // Join failed, create a new room
            PhotonNetwork.JoinRandomOrCreateRoom();
        }


        public void LeaveRoom()
        {
            PhotonNetwork.LeaveRoom();
        }

        public void QuitApplication()
        {
            Application.Quit();
        }


        private void InstantiatePlayer()
        {
            if (_LoginManager != null)
            {
                int genderID = _LoginManager._Avatar.gender_id;
                PhotonNetwork.NickName = _LoginManager.name ;

                if (genderID == 3)
                {
                    InstantiateMalePlayer();
                }
                else if (genderID == 4)
                {
                    InstantiateFemalePlayer();
                }
                else
                {
                    Debug.LogError("Invalid GenderID value.");
                }
            }
            else
            {
                Debug.LogError("LoginManager instance is not available.");
            }


        }

        private void InstantiateMalePlayer()
        {
            if (malePlayerPrefab != null)
            {
                PhotonNetwork.Instantiate(malePlayerPrefab.name, new Vector3(0f, 0f, 0f), Quaternion.identity, 0);
                Debug.Log("Instantiated male player prefab.");
            }
            else
            {
                Debug.LogError("Male player prefab is missing.");
            }
        }

        private void InstantiateFemalePlayer()
        {
            if (femalePlayerPrefab != null)
            {
                PhotonNetwork.Instantiate(femalePlayerPrefab.name, new Vector3(0f, 0f, 0f), Quaternion.identity, 0);
                Debug.Log("Instantiated female player prefab.");
            }
            else
            {
                Debug.LogError("Female player prefab is missing.");
            }
        }

    }
}