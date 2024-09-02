using System;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using Photon.Pun.Demo.PunBasics;
using UnityEngine;
using UnityEngine.Networking;
public class AssetCharactor : MonoBehaviourPunCallbacks
{
    public static AssetCharactor Instance { get; private set; }

    [SerializeField]
    private List<SelectItem._AvatarData> avatarData = new List<SelectItem._AvatarData>();

    public LaunCherTest1 _Launcher;
    public LoginManager _loginManager;
    public entername _Name;
    public List<SelectItem._AvatarData> AvatarData
    {
        get { return avatarData; }
        private set { avatarData = value; }
    }

    public GameObject _LoadingBar;
    public GameObject _LoadingFailed;
    public GameObject _LoadingOK;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this.gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(this.gameObject);

        SelectItem selectItem = FindObjectOfType<SelectItem>();
        selectItem.selectedSex = 3;

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
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


        SaveAvatarDataToStorage();
    }
    private void Update()
    {
        if (_loginManager == null)
        {
            _loginManager = FindObjectOfType<LoginManager>();
        }
        if (_Name == null)
        {
            _Name = FindObjectOfType<entername>();
        }

       /* if (_Launcher == null)
        {
            Debug.Log("tring to find Launcher");
            //_Launcher = FindObjectsOfType<LaunCherTest1>()[0];
        }*/
    }

    ///Not Use///
    public void LoadAvatarDataFromAssetCharactor()
    {
        SelectItem selectItem = FindObjectOfType<SelectItem>(); // Find SelectItem in the scene
        if (selectItem != null)
        {
            bool[] loadedAccessories = new bool[selectItem.Accessories.Length];

            foreach (var data in AvatarData)
            {
                switch (data.Type)
                {
                    case "Hair":
                        selectItem.SelectHair(data.Id);
                        selectItem.ChangeHairColor(data.ColorId);
                        break;
                    case "Shirt":
                        selectItem.SelectShirt(data.Id);
                        selectItem.ChangeShirtColor(data.ColorId);
                        break;
                    case "Pants":
                        selectItem.SelectPants(data.Id);
                        selectItem.ChangePantsColor(data.ColorId);
                        break;
                    case "Shoes":
                        selectItem.SelectShoes(data.Id);
                        selectItem.ChangeShoesColor(data.ColorId);
                        break;
                    case "Face":
                        selectItem.SelectFace(data.Id);
                        break;
                    case "SkinColor":
                        selectItem.ChangeSkinColor(data.Id);
                        break;
                    case "Accessory":
                        if (data.Id >= 0 && data.Id < selectItem.Accessories.Length)
                        {
                            selectItem.Accessories[data.Id].SetActive(true);
                            loadedAccessories[data.Id] = true;
                        }
                        break;
                    case "Sex":
                        selectItem.BodySelection(data.Id);
                        break;
                }
            }

            // Disable accessories that were not loaded from saved data
            for (int i = 0; i < selectItem.Accessories.Length; i++)
            {
                if (!loadedAccessories[i])
                {
                    selectItem.Accessories[i].SetActive(false);
                }
            }
        }
    }


    private List<SelectItem._AvatarData> LoadSavedAvatarData()
    {
        return new List<SelectItem._AvatarData>(); 
    }


    private void SaveAvatarDataToStorage()
    {
        // Implement saving logic to storage
    }

    public void OnClickCreateAvatarButton()
    {
        StartCoroutine(CreateAvatar());
    }

    private IEnumerator CreateAvatar()
    {

        // Add LOADING POPUP HERE
        _LoadingBar.SetActive(true);

        SelectItem selectItem = FindObjectOfType<SelectItem>();

        if (selectItem == null)
        {
            Debug.LogError("SelectItem instance not found in the scene.");
            yield break;
        }

        var Avatar = new Avatar
        {
            name = _Name.username.text,
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
        Debug.Log("SENT THIS CREATE AVATAR-> " + json);

        using var request = new UnityWebRequest("http://13.250.106.216:1000/api/avatar/createAvatar", "POST")
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
            _LoadingFailed.SetActive(true);
            Debug.LogError("Error: " + request.error);
            Debug.LogError("Status Code: " + request.responseCode);
            Debug.LogError("URL: " + request.url);
        }
        else
        {
            Debug.Log("Login Response: " + request.downloadHandler.text);
            var avatarResponse = JsonUtility.FromJson<LoginManager.LoginResponse>(request.downloadHandler.text);

            if (avatarResponse.status)
            {
                Debug.Log("Login successful!");
                _loginManager._Avatar.name = avatarResponse.data.avatar.name;
                _loginManager._Avatar.uid = avatarResponse.data.avatar.uid;
                _loginManager._Avatar.gender_id = avatarResponse.data.avatar.gender_id;
                _loginManager._Avatar.skin_id = avatarResponse.data.avatar.skin_id;
                _loginManager._Avatar.face_id = avatarResponse.data.avatar.face_id;
                _loginManager._Avatar.hair_id = avatarResponse.data.avatar.hair_id;
                _loginManager._Avatar.hair_color_id = avatarResponse.data.avatar.hair_color_id;
                _loginManager._Avatar.shirt_id = avatarResponse.data.avatar.shirt_id;
                _loginManager._Avatar.shirt_color_id = avatarResponse.data.avatar.shirt_color_id;
                _loginManager._Avatar.pant_id = avatarResponse.data.avatar.pant_id;
                _loginManager._Avatar.pant_color_id = avatarResponse.data.avatar.pant_color_id;
                _loginManager._Avatar.shoe_id = avatarResponse.data.avatar.shoe_id;
                _loginManager._Avatar.shoe_color_id =avatarResponse.data.avatar.shoe_color_id;
                _loginManager._Avatar.accessory_id = avatarResponse.data.avatar.accessory_id;

                //_Launcher.Connect();
                _LoadingBar.SetActive(true);
                yield return new WaitForSeconds(3f);

                _LoadingOK.SetActive(true);

                Application.LoadLevel("Game");

                // Add LOADING POPUP HERE
            }
            else
            {
                Debug.LogError("Login failed!");
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
        public string name;
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
        public string accessory_id; // Allow multiple accessories
    }

}


