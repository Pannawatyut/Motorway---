﻿using UnityEngine;
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
            if (_LoginManager == null)
            {
                _LoginManager = FindObjectOfType<LoginManager>();
            }
            if (PhotonNetwork.InRoom && PlayerManager.LocalPlayerInstance == null)
            {
                Debug.LogFormat("We are Instantiating LocalPlayer from {0}", SceneManagerHelper.ActiveSceneName);
                InstantiatePlayer();
            }
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
                Debug.Log("Trying to spawn Player");
                InstantiatePlayer();
            }
        }

        public void OnPlayerEnteredRoom(Player other)
        {
            //Debug.Log("OnPlayerEnteredRoom() " + other.NickName);
            Debug.Log("Player Entered the Room");
            if (PhotonNetwork.IsMasterClient)
            {
                Debug.LogFormat("OnPlayerEnteredRoom IsMasterClient {0}", PhotonNetwork.IsMasterClient);
                LoadArena();
            }
        }

        public void OnPlayerLeftRoom(Player other)
        {
            //Debug.Log("OnPlayerLeftRoom() " + other.NickName);

            if (PhotonNetwork.IsMasterClient)
            {
                Debug.LogFormat("OnPlayerLeftRoom IsMasterClient {0}", PhotonNetwork.IsMasterClient);
                LoadArena();
            }
        }

        public override void OnLeftRoom()
        {
            //SceneManager.LoadScene("MiniGame");
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
            if (_LoginManager != null)
            {
                int genderID = _LoginManager._Avatar.gender_id;

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