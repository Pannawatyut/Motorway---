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
        private void FixedUpdate()
        {
            if (Camera.main)
            {
                this.transform.LookAt(Camera.main.transform);
            }

        }
    }
}
