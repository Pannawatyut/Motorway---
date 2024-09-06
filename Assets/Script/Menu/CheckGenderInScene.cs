using System;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using Photon.Pun.Demo.PunBasics;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.Events;

public class CheckGenderInScene : MonoBehaviour
{
    [SerializeField]
    private List<SelectItem._AvatarData> avatarData = new List<SelectItem._AvatarData>();

    public LaunCherTest1 _Launcher;
    public LoginManager _loginManager;
    public List<SelectItem._AvatarData> AvatarData
    {
        get { return avatarData; }
        private set { avatarData = value; }
    }

    public void OnEnable()
    {
        _SetupUI();
    }

    public UnityEvent _Event_Male;
    public UnityEvent _Event_FeMale;

    public void _SetupUI()
    {

        _loginManager = FindObjectOfType<LoginManager>();

        if (_loginManager._Avatar.gender_id == 3) // MALE
        {
            _Event_Male.Invoke();
            Debug.Log("is MALE");
        }
        else // FEMALE
        {
            _Event_FeMale.Invoke();
            Debug.Log("is Female");
        }
    }

    /////////// Save Avatar ////////////////
    public void SaveAvatarData(SelectItem selectItem)
    {


        AvatarData.Clear();

        // Add avatar data from SelectItem
        AvatarData.Add(new SelectItem._AvatarData { Type = "Hair", Id = selectItem.selectedHairIndex, ColorId = selectItem.selectedHairColorIndex });
        AvatarData.Add(new SelectItem._AvatarData { Type = "Shirt", Id = selectItem.selectedShirtIndex, ColorId = selectItem.selectedShirtColorIndex });
        AvatarData.Add(new SelectItem._AvatarData { Type = "Pants", Id = selectItem.selectedPantsIndex, ColorId = selectItem.selectedPantsColorIndex });
        AvatarData.Add(new SelectItem._AvatarData { Type = "Shoes", Id = selectItem.selectedShoesIndex, ColorId = selectItem.selectedShoesColorIndex });
        AvatarData.Add(new SelectItem._AvatarData { Type = "Face", Id = selectItem.selectedFaceIndex, ColorId = 0 });
        AvatarData.Add(new SelectItem._AvatarData { Type = "SkinColor", Id = selectItem.selectedSkinColor, ColorId = 0 });

        for (int i = 0; i < selectItem.Accessories.Length; i++)
        {
            if (selectItem.Accessories[i].activeSelf)
            {
                AvatarData.Add(new SelectItem._AvatarData { Type = "Accessory", Id = i, ColorId = 0 });
            }
        }

        AvatarData.Add(new SelectItem._AvatarData { Type = "Sex", Id = selectItem.selectedSex, ColorId = 0 });
    }

    public GameObject _LoadingBar;
    public GameObject _LoadingFailed;
    public GameObject _LoadingOK;

    public void OnClickCreateAvatarButton()
    {
        StartCoroutine(CreateAvatar());
    }

    public AvatarResponse avatarResponse;

    private IEnumerator CreateAvatar()
    {
        // ADD LOADING POPUP HERE
        _LoadingBar.SetActive(true);

        SelectItem selectItem = FindObjectOfType<SelectItem>();

        if (selectItem == null)
        {
            Debug.LogError("SelectItem instance not found in the scene.");
            yield break;
        }

        List<int> accessoryIds_Temp = new List<int>();

        for (int i = 0; i < selectItem.Accessories.Length; i++)
        {
            if (selectItem.Accessories[i].activeSelf)
            {
                accessoryIds_Temp.Add(i);
            }
        }

        var Avatar = new Avatar
        {
            uid = _loginManager._Avatar.uid,
            gender_id = selectItem.selectedSex.ToString(),
            skin_id = selectItem.selectedSkinColor.ToString(),
            face_id = selectItem.selectedFaceIndex.ToString(),
            hair_id = selectItem.selectedHairIndex.ToString(),
            hair_color_id = selectItem.selectedHairColorIndex.ToString(),
            shirt_id = selectItem.selectedShirtIndex.ToString(),
            shirt_color_id = selectItem.selectedShirtColorIndex.ToString(),
            pant_id = selectItem.selectedPantsIndex.ToString(),
            pant_color_id = selectItem.selectedPantsColorIndex.ToString(),
            shoe_id = selectItem.selectedShoesIndex.ToString(),
            shoe_color_id = selectItem.selectedShoesColorIndex.ToString(),
            accessory_id = selectItem.selectedAccessoryIndex.ToString(),

        };

        string json = JsonUtility.ToJson(Avatar);
        Debug.Log(json);
        using var request = new UnityWebRequest(LoginManager.Instance._APIURL + "/api/avatar/updateAvatar", "POST")
        {
            uploadHandler = new UploadHandlerRaw(System.Text.Encoding.UTF8.GetBytes(json)),
            downloadHandler = new DownloadHandlerBuffer()
        };

        request.SetRequestHeader("Content-Type", "application/json");
        request.SetRequestHeader("Authorization", _loginManager._Account.access_token);

        yield return request.SendWebRequest();

        _LoadingBar.SetActive(false);

        if (request.result != UnityWebRequest.Result.Success)
        {
            Debug.LogError("Error: " + request.error);
            Debug.LogError("Status Code: " + request.responseCode);
            Debug.LogError("URL: " + request.url);
            _LoadingFailed.SetActive(true);
        }
        else
        {
            Debug.Log("SAVE AVATAR - UPDATE API : " + request.downloadHandler.text);
            avatarResponse = JsonUtility.FromJson<AvatarResponse>(request.downloadHandler.text);

            if (avatarResponse.status)
            {
                _loginManager._Avatar.gender_id = int.Parse(avatarResponse.data.avatar.gender_id);
                _loginManager._Avatar.skin_id = int.Parse(avatarResponse.data.avatar.skin_id);
                _loginManager._Avatar.face_id = int.Parse(avatarResponse.data.avatar.face_id);
                _loginManager._Avatar.hair_id = int.Parse(avatarResponse.data.avatar.hair_id);
                _loginManager._Avatar.hair_color_id = int.Parse(avatarResponse.data.avatar.hair_color_id);
                _loginManager._Avatar.shirt_id = int.Parse(avatarResponse.data.avatar.shirt_id);
                _loginManager._Avatar.shirt_color_id = int.Parse(avatarResponse.data.avatar.shirt_color_id);
                _loginManager._Avatar.pant_id = int.Parse(avatarResponse.data.avatar.pant_id);
                _loginManager._Avatar.pant_color_id = int.Parse(avatarResponse.data.avatar.pant_color_id);
                _loginManager._Avatar.shoe_id = int.Parse(avatarResponse.data.avatar.shoe_id);
                _loginManager._Avatar.shoe_color_id = int.Parse(avatarResponse.data.avatar.shoe_color_id);
                _loginManager._Avatar.accessory_id = int.Parse(avatarResponse.data.avatar.accessory_id);

                _LoadingOK.SetActive(true);
                yield return new WaitForSeconds(3);

                Application.LoadLevel("Game");
            }
            else
            {
                Debug.Log("SAVE AVATAR FAILED - UPDATE API : " + request.downloadHandler.text);
                // ADD FAILED POPUP HERE
            }
        }
    }


    public Avatar _avatar;

    [System.Serializable]
    public class AvatarResponse
    {
        public bool status;
        public Data data;
    }

    [System.Serializable]
    public class Data
    {
        public Avatar avatar;
    }

    [System.Serializable]
    public class Avatar
    {
        public string uid;
        public string gender_id;
        public string skin_id;
        public string face_id;
        public string hair_id;
        public string hair_color_id;
        public string shirt_id;
        public string shirt_color_id;
        public string pant_id;
        public string pant_color_id;
        public string shoe_id;
        public string shoe_color_id;
        public string accessory_id; 
    }

}
