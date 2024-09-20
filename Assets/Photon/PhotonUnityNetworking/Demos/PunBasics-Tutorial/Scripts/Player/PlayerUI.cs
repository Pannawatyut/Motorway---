using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace Photon.Pun.Demo.PunBasics
{
#pragma warning disable 649

    /// <summary>
    /// Player UI. Constraint the UI to follow a PlayerManager GameObject in the world,
    /// Affect a slider and text to display Player's name and health
    /// </summary>
    /// 

    public class PlayerUI : MonoBehaviour
    {
        public GameObject _Cam;
        bool _isReady;
        public void OnEnable()
        {
            StartCoroutine(_FindMainCam());
        }

        IEnumerator _FindMainCam()
        {
            yield return new WaitForSeconds(0.5f);

            _isReady = true;
        }
        private void FixedUpdate()
        {
            if(_isReady)
            {
                if (_Cam)
                {
                    this.transform.LookAt(_Cam.transform);
                }
                else
                {
                    if(Camera.main != null)
                    {
                        _Cam = Camera.main.gameObject;
                    }
                    else
                    {
                        Debug.LogWarning("NO MAIN CAMERA IN SCENE");
                    }
                    
                }
            }
            

        }
    }
}
