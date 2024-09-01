using UnityEngine;
using Photon.Pun;
using Photon.Realtime;



namespace Photon.Pun.Demo.PunBasics
{
    public class LaunCherTest1 : MonoBehaviourPunCallbacks
    {
       /* public static LaunCherTest1 Instance { get; private set; }
        // Max players per room, configurable via the inspector or here
        [SerializeField] private byte maxPlayersPerRoom = 4;

        private bool isConnecting;
        private string gameVersion = "1";

        void Awake()
        {
            // Automatically sync scene across networked clients
            if (Instance == null)
            {
                Instance = new LaunCherTest1();
            }
            else
            {
                Destroy(Instance);
            }
            PhotonNetwork.AutomaticallySyncScene = true;
        }

        public void Connect()
        {
            isConnecting = true;

            if (PhotonNetwork.IsConnected)
            {
                PhotonNetwork.JoinRandomRoom();
            }
            else
            {
                PhotonNetwork.GameVersion = this.gameVersion;
                PhotonNetwork.ConnectUsingSettings();
            }
        }

        public override void OnConnectedToMaster()
        {
            if (isConnecting)
            {
                PhotonNetwork.JoinRandomRoom();
            }
        }

        public override void OnJoinRandomFailed(short returnCode, string message)
        {
            // Join failed, create a new room
            PhotonNetwork.CreateRoom(null, new RoomOptions { MaxPlayers = this.maxPlayersPerRoom });
        }

        public override void OnDisconnected(DisconnectCause cause)
        {
            isConnecting = false;
            Debug.LogWarning("Disconnected: " + cause);
        }

        public override void OnJoinedRoom()
        {
            Debug.Log("Joined Room with " + PhotonNetwork.CurrentRoom.PlayerCount + " player(s).");
            this.GetComponent<LaunCherTest1>().enabled = false;
            if (PhotonNetwork.CurrentRoom.PlayerCount == 1)
            {
                // Load the main game scene
                
            }
        }*/

    }

}
