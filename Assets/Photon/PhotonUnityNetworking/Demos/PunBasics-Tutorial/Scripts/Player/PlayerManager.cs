using UnityEngine;
using UnityEngine.EventSystems;

namespace Photon.Pun.Demo.PunBasics
{
    #pragma warning disable 649

    /// <summary>
    /// Player manager.
    /// Handles fire Input and Beams.
    /// </summary>
    public class PlayerManager : MonoBehaviourPunCallbacks, IPunObservable
    {
        #region Public Fields

        [Tooltip("The current Health of our player")]
        public float Health = 1f;

        [Tooltip("The local player instance. Use this to know if the local player is represented in the Scene")]
        public static GameObject LocalPlayerInstance;

        #endregion

        #region Private Fields

        [Tooltip("The Player's UI GameObject Prefab")]
        [SerializeField]
        private GameObject playerUiPrefab;

        [Tooltip("The Beams GameObject to control")] [SerializeField]
        //private GameObject beams;
        public int viewID;

        //True, when the user is firing
        bool IsFiring;

        #endregion

        public void Awake()
        {
            if (photonView.IsMine)
            {
                LocalPlayerInstance = gameObject;
            }

            DontDestroyOnLoad(gameObject);

            viewID = photonView.ViewID;
        }

        public void Start()
        {
            // No need to instantiate the UI prefab, we assume it is already part of the player prefab.
            if (this.playerUiPrefab != null)
            {
                // Find the UILookAt component in the UI prefab
                var uiComponents = playerUiPrefab.GetComponentsInChildren<UILookAt>(true);
                foreach (var ui in uiComponents)
                {
                    ui.targetCamera = Camera.main;
                    ui.playerTransform = this.transform;
                }

                // Send the "this" instance to the SetTarget method of the existing UI prefab.
                playerUiPrefab.SendMessage("SetTarget", this, SendMessageOptions.RequireReceiver);
            }

            #if UNITY_5_4_OR_NEWER
            UnityEngine.SceneManagement.SceneManager.sceneLoaded += OnSceneLoaded;
            #endif
        }

        public override void OnDisable()
        {
            base.OnDisable();

            #if UNITY_5_4_OR_NEWER
            UnityEngine.SceneManagement.SceneManager.sceneLoaded -= OnSceneLoaded;
            #endif
        }

        private bool leavingRoom;

        public void Update()
        {
            if (photonView.IsMine)
            {
                this.ProcessInputs();

                if (this.Health <= 0f && !this.leavingRoom)
                {
                    this.leavingRoom = PhotonNetwork.LeaveRoom();
                }
            }
        }

        public override void OnLeftRoom()
        {
            this.leavingRoom = false;
        }

        public void OnTriggerEnter(Collider other)
        {
            if (!photonView.IsMine)
            {
                return;
            }

            if (!other.name.Contains("Beam"))
            {
                return;
            }

            this.Health -= 0.1f;
        }

        public void OnTriggerStay(Collider other)
        {
            if (!photonView.IsMine)
            {
                return;
            }

            if (!other.name.Contains("Beam"))
            {
                return;
            }

            this.Health -= 0.1f * Time.deltaTime;
        }

        #if !UNITY_5_4_OR_NEWER
        void OnLevelWasLoaded(int level)
        {
            this.CalledOnLevelWasLoaded(level);
        }
        #endif

        void CalledOnLevelWasLoaded(int level)
        {
            if (!Physics.Raycast(transform.position, -Vector3.up, 5f))
            {
                transform.position = new Vector3(0f, 5f, 0f);
            }

            if (this.playerUiPrefab != null)
            {
                // Find the UILookAt component in the UI prefab
                var uiComponents = playerUiPrefab.GetComponentsInChildren<UILookAt>(true);
                foreach (var ui in uiComponents)
                {
                    ui.targetCamera = Camera.main;
                    ui.playerTransform = this.transform;
                }

                // Send the "this" instance to the SetTarget method of the existing UI prefab.
                playerUiPrefab.SendMessage("SetTarget", this, SendMessageOptions.RequireReceiver);
            }
            else
            {
                Debug.LogWarning("<Color=Red><b>Missing</b></Color> PlayerUiPrefab reference on player Prefab.", this);
            }
        }

        #if UNITY_5_4_OR_NEWER
        void OnSceneLoaded(UnityEngine.SceneManagement.Scene scene, UnityEngine.SceneManagement.LoadSceneMode loadingMode)
        {
            this.CalledOnLevelWasLoaded(scene.buildIndex);
        }
        #endif

        void ProcessInputs()
        {
            if (Input.GetButtonDown("Fire1"))
            {
                if (EventSystem.current.IsPointerOverGameObject())
                {
                    // return;
                }

                if (!this.IsFiring)
                {
                    this.IsFiring = true;
                }
            }

            if (Input.GetButtonUp("Fire1"))
            {
                if (this.IsFiring)
                {
                    this.IsFiring = false;
                }
            }
        }

        public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
        {
            if (stream.IsWriting)
            {
                stream.SendNext(this.IsFiring);
                stream.SendNext(this.Health);
            }
            else
            {
                this.IsFiring = (bool)stream.ReceiveNext();
                this.Health = (float)stream.ReceiveNext();
            }
        }
    }
}
