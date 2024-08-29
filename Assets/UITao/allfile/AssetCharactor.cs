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

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this.gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(this.gameObject);

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
        SelectItem selectItem = FindObjectOfType<SelectItem>();

        if (selectItem == null)
        {
            Debug.LogError("SelectItem instance not found in the scene.");
            yield break;
        }
        var Avatar = new Avatar
        {
            name = _Name.username.text,
            gender_id = selectItem.selectedSex,
            skin_id = selectItem.selectedSkinColor,
            face_id = selectItem.selectedFaceIndex,
            hair_id = selectItem.selectedHairIndex,
            hair_color_id = selectItem.selectedHairColorIndex,
            shirt_id = selectItem.selectedShirtIndex,
            shirt_color_id = selectItem.selectedShirtColorIndex,
            pant_id = selectItem.selectedPantsIndex,
            pant_color_id = selectItem.selectedPantsColorIndex,
            shoe_id = selectItem.selectedShoesIndex,
            shoe_color_id = selectItem.selectedShoesColorIndex,
            accessory_id = selectItem.selectedAccessoryIndex
        };

        // แปลงอ็อบเจ็กต์การล็อกอินเป็น JSON
        string json = JsonUtility.ToJson(Avatar);

        // สร้าง UnityWebRequest สำหรับ POST method
        using var request = new UnityWebRequest("http://13.250.106.216:1000/api/avatar/createAvatar", "POST")
        {
            uploadHandler = new UploadHandlerRaw(System.Text.Encoding.UTF8.GetBytes(json)),
            downloadHandler = new DownloadHandlerBuffer()
        };

        // ตั้งค่า header ของคำขอ
        request.SetRequestHeader("Content-Type", "application/json");
        request.SetRequestHeader("Authorization", _loginManager._Account.access_token);

        // ส่งคำขอและรอรับการตอบกลับ
        yield return request.SendWebRequest();

        // ตรวจสอบผลลัพธ์ของคำขอ
        if (request.result != UnityWebRequest.Result.Success)
        {
            Debug.LogError("Error: " + request.error);
            Debug.LogError("Status Code: " + request.responseCode);
            Debug.LogError("URL: " + request.url);
        }
        else
        {
            Debug.Log("Login Response: " + request.downloadHandler.text);

            // แปลงข้อมูลตอบกลับเป็นอ็อบเจ็กต์
            var avatarResponse = JsonUtility.FromJson<AvatarResponse>(request.downloadHandler.text);

            if (avatarResponse.status)
            {
                Debug.Log("Login successful!");
                // ทำการเก็บข้อมูล token หรือข้อมูลอื่นๆ จากการล็อกอิน
                string name = string.IsNullOrEmpty(avatarResponse.data.avatar.name) ? "-" : avatarResponse.data.avatar.name;
                string gender_id = string.IsNullOrEmpty(avatarResponse.data.avatar.gender_id.ToString()) ? "-" : avatarResponse.data.avatar.gender_id.ToString();
                string skin_id = string.IsNullOrEmpty(avatarResponse.data.avatar.skin_id.ToString()) ? "-" : avatarResponse.data.avatar.skin_id.ToString();
                string face_id = string.IsNullOrEmpty(avatarResponse.data.avatar.face_id.ToString()) ? "-" : avatarResponse.data.avatar.face_id.ToString();
                string hair_id = string.IsNullOrEmpty(avatarResponse.data.avatar.hair_id.ToString()) ? "-" : avatarResponse.data.avatar.hair_id.ToString();
                string hair_color_id = string.IsNullOrEmpty(avatarResponse.data.avatar.hair_color_id.ToString()) ? "-" : avatarResponse.data.avatar.hair_color_id.ToString();
                string shirt_id = string.IsNullOrEmpty(avatarResponse.data.avatar.shirt_id.ToString()) ? "-" : avatarResponse.data.avatar.shirt_id.ToString();
                string shirt_color_id = string.IsNullOrEmpty(avatarResponse.data.avatar.shirt_color_id.ToString()) ? "-" : avatarResponse.data.avatar.shirt_color_id.ToString();
                string pant_id = string.IsNullOrEmpty(avatarResponse.data.avatar.pant_id.ToString()) ? "-" : avatarResponse.data.avatar.pant_id.ToString();
                string pant_color_id = string.IsNullOrEmpty(avatarResponse.data.avatar.pant_color_id.ToString()) ? "-" : avatarResponse.data.avatar.pant_color_id.ToString();
                string shoe_id = string.IsNullOrEmpty(avatarResponse.data.avatar.shoe_id.ToString()) ? "-" : avatarResponse.data.avatar.shoe_id.ToString();
                string shoe_color_Id = string.IsNullOrEmpty(avatarResponse.data.avatar.shoe_color_id.ToString()) ? "-" : avatarResponse.data.avatar.shoe_color_id.ToString();
                string accessory_id = string.IsNullOrEmpty(avatarResponse.data.avatar.accessory_id.ToString()) ? "-" : avatarResponse.data.avatar.accessory_id.ToString();

                _avatar.name = name;
                _avatar.gender_id = avatarResponse.data.avatar.gender_id;
                _avatar.skin_id = avatarResponse.data.avatar.skin_id;
                _avatar.face_id = avatarResponse.data.avatar.face_id;
                _avatar.hair_id = avatarResponse.data.avatar.hair_id;
                _avatar.hair_color_id = avatarResponse.data.avatar.hair_color_id;
                _avatar.shirt_id = avatarResponse.data.avatar.shirt_id;
                _avatar.shirt_color_id = avatarResponse.data.avatar.shirt_color_id;
                _avatar.pant_id = avatarResponse.data.avatar.pant_id;
                _avatar.pant_color_id = avatarResponse.data.avatar.pant_color_id;
                _avatar.shoe_id = avatarResponse.data.avatar.shoe_id;
                _avatar.shoe_color_id = avatarResponse.data.avatar.shoe_color_id;
                _avatar.accessory_id = avatarResponse.data.avatar.accessory_id;

                _Launcher.Connect();
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
        public string uid;
        public string name;
        public int gender_id;
        public int skin_id;
        public int face_id;
        public int hair_id;
        public int hair_color_id;
        public int shirt_id;
        public int shirt_color_id;
        public int pant_id;
        public int pant_color_id;
        public int shoe_id;
        public int shoe_color_id;
        public int accessory_id;
    }
}


