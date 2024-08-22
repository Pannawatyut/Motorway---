using UnityEngine;
using UnityEngine.UI;

namespace Photon.Pun.Demo.PunBasics
{
    #pragma warning disable 649

    /// <summary>
    /// Player UI. Constraint the UI to follow a PlayerManager GameObject in the world,
    /// Affect a slider and text to display Player's name and health
    /// </summary>
    public class PlayerUI : MonoBehaviour
    {
        #region Private Fields

        [Tooltip("Pixel offset from the player target")]
        [SerializeField]
        private Vector3 screenOffset = new Vector3(0f, 30f, 0f);

        [Tooltip("UI Text to display Player's Name")]
        [SerializeField]
        private Text playerNameText;

        
        [SerializeField]
        private PlayerManager target;
        private Transform targetTransform;
        private Renderer targetRenderer;
        private CanvasGroup _canvasGroup;
       

        #endregion

        #region MonoBehaviour Messages

        /// <summary>
        /// MonoBehaviour method called on GameObject by Unity during early initialization phase
        /// </summary>
        void Awake()
        {
            _canvasGroup = this.GetComponent<CanvasGroup>();
            // Set the Canvas as a child of the main Canvas and keep the local position/rotation
        }

        /// <summary>
        /// MonoBehaviour method called on GameObject by Unity on every frame.
        /// Update the health slider to reflect the Player's health
        /// </summary>
        void Update()
        {

            // Destroy itself if the target is null, It's a fail safe when Photon is destroying Instances of a Player over the network
            if (target == null) {
                Destroy(this.gameObject);
                return;
            }
            
        }

        /// <summary>
        /// MonoBehaviour method called after all Update functions have been called. This is useful to order script execution.
        /// In our case since we are following a moving GameObject, we need to proceed after the player was moved during a particular frame.
        /// </summary>
        void LateUpdate ()
        {
            // Do not show the UI if we are not visible to the camera, thus avoid potential bugs with seeing the UI, but not the player itself.
            if (targetRenderer != null) {
                _canvasGroup.alpha = targetRenderer.isVisible ? 1f : 0f;
            }

          
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Assigns a Player Target to Follow and represent.
        /// </summary>
        /// <param name="target">Target.</param>
        public void SetTarget(PlayerManager _target)
        {
            if (_target == null) {
                Debug.LogError("<Color=Red><b>Missing</b></Color> PlayerManager target for PlayerUI.SetTarget.", this);
                return;
            }

            // Cache references for efficiency because we are going to reuse them.
            this.target = _target;
            targetTransform = this.target.GetComponent<Transform>();
            targetRenderer = this.target.GetComponentInChildren<Renderer>();

            if (playerNameText != null) {
                playerNameText.text = this.target.photonView.Owner.NickName;
            }
            
        }

        #endregion
    }
}
