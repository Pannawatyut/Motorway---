using UnityEngine;
using UnityEngine.SceneManagement;
using Photon.Realtime;
using Photon.Pun;

namespace Photon.Pun.Demo.PunBasics
{
    public class GameManager : MonoBehaviourPunCallbacks
    {
        public static GameManager Instance;

        private GameObject instance;

        [Tooltip("The male prefab to use for representing the player")]
        [SerializeField]
        private GameObject malePlayerPrefab;

        [Tooltip("The female prefab to use for representing the player")]
        [SerializeField]
        private GameObject femalePlayerPrefab;

        void Start()
        {
            Instance = this;

            if (!PhotonNetwork.IsConnected)
            {
                SceneManager.LoadScene("StarGame");
                return;
            }

            if (malePlayerPrefab == null || femalePlayerPrefab == null)
            {
                Debug.LogError("Missing playerPrefab Reference. Please set it up in GameObject 'Game Manager'", this);
            }
            else
            {
                if (PhotonNetwork.InRoom && PlayerManager.LocalPlayerInstance == null)
                {
                    Debug.LogFormat("We are Instantiating LocalPlayer from {0}", SceneManagerHelper.ActiveSceneName);
                    InstantiatePlayer();
                }
                else
                {
                    Debug.LogFormat("Ignoring scene load for {0}", SceneManagerHelper.ActiveSceneName);
                }
            }
        }

        void Update()
        {
            // if (Input.GetKeyDown(KeyCode.Escape))
            // {
            //     QuitApplication();
            //     
            // }
            // if (Input.GetKeyDown(KeyCode.G))
            // {
            //     // Handle leave room and scene loading logic
            //     PhotonNetwork.LeaveRoom();
            //     SceneManager.LoadScene(3);
            // }
        }

        public override void OnJoinedRoom()
        {
            if (PlayerManager.LocalPlayerInstance == null)
            {
                InstantiatePlayer();
            }
        }

        public override void OnPlayerEnteredRoom(Player other)
        {
            Debug.Log("OnPlayerEnteredRoom() " + other.NickName);

            if (PhotonNetwork.IsMasterClient)
            {
                Debug.LogFormat("OnPlayerEnteredRoom IsMasterClient {0}", PhotonNetwork.IsMasterClient);
                LoadArena();
            }
        }

        public override void OnPlayerLeftRoom(Player other)
        {
            Debug.Log("OnPlayerLeftRoom() " + other.NickName);

            if (PhotonNetwork.IsMasterClient)
            {
                Debug.LogFormat("OnPlayerLeftRoom IsMasterClient {0}", PhotonNetwork.IsMasterClient);
                LoadArena();
            }
        }

        public override void OnLeftRoom()
        {
            SceneManager.LoadScene("MiniGame");
        }

        public void LeaveRoom()
        {
            PhotonNetwork.LeaveRoom();
        }

        public void QuitApplication()
        {
            Application.Quit();
        }

        private void LoadArena()
        {
            if (!PhotonNetwork.IsMasterClient)
            {
                Debug.LogError("PhotonNetwork : Trying to Load a level but we are not the master Client");
                return;
            }

            Debug.LogFormat("PhotonNetwork : Loading Level : {0}", PhotonNetwork.CurrentRoom.PlayerCount);
            //PhotonNetwork.LoadLevel("Game");
        }

        private void InstantiatePlayer()
        {
            if (PhotonNetwork.LocalPlayer.CustomProperties.TryGetValue("selectedSex", out object selectedSexObj))
            {
                int selectedSex = (int)selectedSexObj;
                if (selectedSex == 3)
                {
                    InstantiateMalePlayer();
                }
                else if (selectedSex == 4)
                {
                    InstantiateFemalePlayer();
                }
                else
                {
                    Debug.LogError("Invalid selectedSex value.");
                }
            }
            else
            {
                Debug.LogError("selectedSex not found in custom properties.");
            }
        }

        private void InstantiateMalePlayer()
        {
            if (malePlayerPrefab != null)
            {
                PhotonNetwork.Instantiate(malePlayerPrefab.name, new Vector3(0f, 0.5f, -0.6f), Quaternion.identity, 0);
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
                PhotonNetwork.Instantiate(femalePlayerPrefab.name, new Vector3(0f, 0.5f, -0.6f), Quaternion.identity, 0);
                Debug.Log("Instantiated female player prefab.");
            }
            else
            {
                Debug.LogError("Female player prefab is missing.");
            }
        }

    }
}