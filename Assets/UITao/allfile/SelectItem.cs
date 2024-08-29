using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using ExitGames.Client.Photon;
using UnityEngine.SceneManagement;
using Photon.Pun.Demo.PunBasics;
using UnityEngine.UI;
using TMPro;
public class SelectItem : MonoBehaviourPunCallbacks
{

    public Imagboyselect imagboyselect;
    public GameObject[] Hair;
    public Material HairMaterial;
    public Color[] HairColors;
    public GameObject[] Accessories;
    public GameObject[] Shirt;
    public Material ShirtMaterial;
    public GameObject[] Pants;
    public Material PantsMaterial;
    public GameObject[] Shoes;
    public Material ShoesMaterial;
    public GameObject[] Face;

    public GameObject[] gender;
    public GameObject[] SelectImagesAccessory;

    public TextMeshProUGUI PlayerName;

    public Color[] ColorsBody;
    public Material Skinbody;

    public int selectedHairIndex = 0;
    public int selectedHairColorIndex = 0;
    public int selectedShirtIndex = 0;
    public int selectedShirtColorIndex = 0;
    public int selectedPantsIndex = 0;
    public int selectedPantsColorIndex = 0;
    public int selectedShoesIndex = 0;
    public int selectedShoesColorIndex = 0;
    public int selectedFaceIndex = 0;
    public int selectedAccessoryIndex = 0;
    public int selectedSkinColor = 0;
    public int selectedSex = 0;
    public string Name;

    public int sexcolor; // chang to shirt Men & Women This ID Shrit 4
    public SkinnedMeshRenderer shirtRenderer;
    public SkinnedMeshRenderer shoesRenderer;

    public LoginManager _loginManager;

    public bool isInitialized = false;

    [System.Serializable]
    public class _AvatarData
    {
        public string Type;
        public int Id;
        public int ColorId;
    }

    public List<_AvatarData> avatarData = new List<_AvatarData>();

    public override void OnEnable()
    {
        _RPCName(_loginManager._Avatar.name);
    }
    public void _RPCName(string _Name)
    {
        photonView.RPC("_LoadName", RpcTarget.OthersBuffered, _Name);
    }

    [PunRPC]
    public void _LoadName(string _Name)
    {
        PlayerName.text = _Name;
    }
    void Start()
    {
        BodySelection(selectedSex);
        if (photonView.IsMine)
        {
            if (_loginManager == null)
            {
                _loginManager = FindObjectOfType<LoginManager>();
            }


            
            //////////////////////////////// Load Avatar To Script Name AssetCharactor //////////////////////////////////////////////////////////// 
            //int hairId = AssetCharactor.Instance.AvatarData.Find(d => d.Type == "Hair")?.Id ?? selectedHairIndex;
            //int hairColorId = AssetCharactor.Instance.AvatarData.Find(d => d.Type == "Hair")?.ColorId ?? selectedHairColorIndex;

            //int faceId = AssetCharactor.Instance.AvatarData.Find(d => d.Type == "Face")?.Id ?? selectedFaceIndex;
            //int faceColorId = AssetCharactor.Instance.AvatarData.Find(d => d.Type == "Face")?.ColorId ?? 0; // Default face color

            //int shirtId = AssetCharactor.Instance.AvatarData.Find(d => d.Type == "Shirt")?.Id ?? selectedShirtIndex;
            //int shirtColorId = AssetCharactor.Instance.AvatarData.Find(d => d.Type == "Shirt")?.ColorId ?? selectedShirtColorIndex;

            //int pantsId = AssetCharactor.Instance.AvatarData.Find(d => d.Type == "Pants")?.Id ?? selectedPantsIndex;
            //int pantsColorId = AssetCharactor.Instance.AvatarData.Find(d => d.Type == "Pants")?.ColorId ?? selectedPantsColorIndex;

            //int shoesId = AssetCharactor.Instance.AvatarData.Find(d => d.Type == "Shoes")?.Id ?? selectedShoesIndex;
            //int shoesColorId = AssetCharactor.Instance.AvatarData.Find(d => d.Type == "Shoes")?.ColorId ?? selectedShoesColorIndex;

            //// Collect all accessory IDs and colors
            //List<int> accessoryIds = new List<int>();
            //List<int> accessoryColorIds = new List<int>();
            //foreach (var data in AssetCharactor.Instance.AvatarData)
            //{
            //    if (data.Type == "Accessory")
            //    {
            //        accessoryIds.Add(data.Id);
            //        accessoryColorIds.Add(data.ColorId);
            //    }
            //}

            //int skincolorID = AssetCharactor.Instance.AvatarData.Find(d => d.Type == "SkinColor")?.Id ?? selectedSkinColor;

            //int sexId = AssetCharactor.Instance.AvatarData.Find(d => d.Type == "Sex")?.Id ?? selectedSex;

            //// Call RPC to initialize avatar for all players
            //photonView.RPC("InitializeAvatar", RpcTarget.AllBuffered, 
            //    hairId, hairColorId, faceId, faceColorId,
            //    shirtId, shirtColorId, pantsId, pantsColorId,
            //    shoesId, shoesColorId, accessoryIds,
            //    sexId, skincolorID);
        }

    }

    
    private void LoadSavedAvatarData()
    {
        bool[] loadedAccessories = new bool[Accessories.Length];
        foreach (var data in avatarData)
        {
            switch (data.Type)
            {
                case "Hair":
                    selectedHairIndex = data.Id;
                    selectedHairColorIndex = data.ColorId;
                    break;
                case "Face":
                    selectedFaceIndex = data.Id;
                    break;
                case "Shirt":
                    selectedShirtIndex = data.Id;
                    selectedShirtColorIndex = data.ColorId;
                    break;
                case "Pants":
                    selectedPantsIndex = data.Id;
                    selectedPantsColorIndex = data.ColorId;
                    break;
                case "Shoes":
                    selectedShoesIndex = data.Id;
                    selectedShoesColorIndex = data.ColorId;
                    break;
                case "Accessory":
                    if (data.Id >= 0 && data.Id < Accessories.Length)
                    {
                        Accessories[data.Id].SetActive(true);
                        loadedAccessories[data.Id] = true;
                    }
                    break;
                case "SkinColor":
                    selectedSkinColor = data.Id;
                    break;
                case "Sex":
                    selectedSex = data.Id;
                    break;
            }
        }

        // Disable accessories that were not loaded from saved data
        for (int i = 0; i < Accessories.Length; i++)
        {
            if (!loadedAccessories[i])
            {
                Accessories[i].SetActive(false);
            }
        }

        ApplyLoadedAvatarData();
    }

    private void ApplyLoadedAvatarData()
    {
        Debug.Log($"Applying loaded avatar data: " +
              $"HairIndex={selectedHairIndex}, HairColorIndex={selectedHairColorIndex}, " +
              $"FaceIndex={selectedFaceIndex}, ShirtIndex={selectedShirtIndex}, ShirtColorIndex={selectedShirtColorIndex}, " +
              $"PantsIndex={selectedPantsIndex}, PantsColorIndex={selectedPantsColorIndex}, " +
              $"ShoesIndex={selectedShoesIndex}, ShoesColorIndex={selectedShoesColorIndex}, " +
              $"AccessoryIndex={selectedAccessoryIndex}, Sex={selectedSex}, SkinColor={selectedSkinColor}");
        ChangeSkinColor(selectedSkinColor);
        SelectHair(selectedHairIndex);
        ChangeHairColor(selectedHairColorIndex);
        SelectFace(selectedFaceIndex);
        SelectShirt(selectedShirtIndex);
        ChangeShirtColor(selectedShirtColorIndex);
        SelectPants(selectedPantsIndex);
        ChangePantsColor(selectedPantsColorIndex);
        SelectShoes(selectedShoesIndex);
        ChangeShoesColor(selectedShoesColorIndex);
        //ToggleAccessory(selectedAccessoryIndex);
        BodySelection(selectedSex);
        
    }

    private void Update()
    {
        if (_loginManager != null && isInitialized == false)
        {
            isInitialized = true;
            selectedHairIndex = _loginManager._Avatar.hair_id;
            selectedHairColorIndex = _loginManager._Avatar.hair_color_id;
            selectedShirtIndex = _loginManager._Avatar.shirt_id;
            selectedShirtColorIndex = _loginManager._Avatar.shirt_color_id;
            selectedPantsIndex = _loginManager._Avatar.pant_id;
            selectedPantsColorIndex = _loginManager._Avatar.pant_color_id;
            selectedShoesIndex = _loginManager._Avatar.shoe_id;
            selectedShoesColorIndex = _loginManager._Avatar.shoe_color_id;
            selectedFaceIndex = _loginManager._Avatar.face_id;
            selectedAccessoryIndex = _loginManager._Avatar.accessory_ids;
            selectedSkinColor = _loginManager._Avatar.skin_id;
            selectedSex = _loginManager._Avatar.gender_id;
            Name = _loginManager._Avatar.name;
            PlayerName.text = Name;
            //BodySelection(selectedSex);
            SelectHair(selectedHairIndex);
            SelectShirt(selectedShirtIndex);
            SelectPants(selectedPantsIndex);
            SelectShoes(selectedShoesIndex);
            SelectFace(selectedFaceIndex);
            ChangeSkinColor(selectedSkinColor);

            ChangeShirtColor(selectedShirtColorIndex);
            ChangePantsColor(selectedPantsColorIndex);
            ChangeShoesColor(selectedShoesColorIndex);

            photonView.RPC("InitializeAvatar", RpcTarget.AllBuffered,
    selectedHairIndex, selectedHairColorIndex, selectedFaceIndex, selectedFaceIndex,
    selectedShirtIndex, selectedShirtColorIndex, selectedPantsIndex, selectedPantsColorIndex,
    selectedShoesIndex, selectedShoesColorIndex, selectedAccessoryIndex,
    selectedSex, selectedSkinColor,Name);

            photonView.RPC("UpdatePlayerNameText", RpcTarget.OthersBuffered, Name);
        }
    }

    [PunRPC]
    void UpdatePlayerNameText(string playerName)
    {
        PlayerName.text = Name;
    }

    public void RandomAvatar()
    {
        // Randomly select skin color
        selectedSkinColor = Random.Range(0, ColorsBody.Length);
        ChangeSkinColor(selectedSkinColor);

        // Randomly select hair and its color
        selectedHairIndex = Random.Range(0, Hair.Length);
        SelectHair(selectedHairIndex);
        selectedHairColorIndex = Random.Range(0, HairColors.Length);
        ChangeHairColor(selectedHairColorIndex);

        // Randomly select shirt and its color
        selectedShirtIndex = Random.Range(0, Shirt.Length);
        SelectShirt(selectedShirtIndex);
        selectedShirtColorIndex = Random.Range(0, HairColors.Length); // Assuming shirt colors are the same as hair colors
        ChangeShirtColor(selectedShirtColorIndex);

        // Randomly select pants and their color
        selectedPantsIndex = Random.Range(0, Pants.Length);
        SelectPants(selectedPantsIndex);
        selectedPantsColorIndex = Random.Range(0, HairColors.Length); // Assuming pants colors are the same as hair colors
        ChangePantsColor(selectedPantsColorIndex);

        // Randomly select shoes and their color
        selectedShoesIndex = Random.Range(0, Shoes.Length);
        SelectShoes(selectedShoesIndex);
        selectedShoesColorIndex = Random.Range(0, HairColors.Length); // Assuming shoes colors are the same as hair colors
        ChangeShoesColor(selectedShoesColorIndex);

        // Randomly select face
        selectedFaceIndex = Random.Range(0, Face.Length);
        SelectFace(selectedFaceIndex);

        // Randomly toggle accessories
        selectedAccessoryIndex = Random.Range(0, Accessories.Length);
        ToggleAccessory(selectedAccessoryIndex);
        

    }


    public void ChangeSkinColor(int colorIndex)
    {
        if (colorIndex >= 0 && colorIndex < ColorsBody.Length)
        {
            Skinbody.color = ColorsBody[colorIndex];
            selectedSkinColor = colorIndex;

        }
    }

    public void SelectHair(int index)
    {
        Debug.Log("select hair" + index);
        if (index >= 0 && index < Hair.Length)
        {
            for (int i = 0; i < Hair.Length; i++)
            {
                Hair[i].SetActive(i == index);
                if (i == index)
                {
                    Renderer hairRenderer = Hair[i].GetComponent<Renderer>();
                    if (hairRenderer != null)
                    {
                        HairMaterial = hairRenderer.material;
                    }
                }
            }
            selectedHairIndex = index;
            
        }
        else
        {
            ChangeHairColor(6);
        }
    }

    public void ChangeHairColor(int colorIndex)
    {
        if (colorIndex >= 0 && colorIndex < HairColors.Length)
        {
            HairMaterial.color = HairColors[colorIndex];
            selectedHairColorIndex = colorIndex;

        }
    }

    public void ToggleAccessory(int index)
    {
        if (index >= 0 && index < Accessories.Length)
        {
            if (index == selectedAccessoryIndex)
            {
                bool isActive = Accessories[index].activeSelf;
                Accessories[index].SetActive(!isActive);
                SelectImagesAccessory[index].SetActive(!isActive);
            }
            else
            {
                Accessories[index].SetActive(true);
                SelectImagesAccessory[index].SetActive(true);
                selectedAccessoryIndex = index;
            }
        }
    }

    public void SelectShirt(int index)
    {
        if (index >= 0 && index < Shirt.Length)
        {
            for (int i = 0; i < Shirt.Length; i++)
            {
                Shirt[i].SetActive(i == index);
                if (i == index)
                {
                    Renderer shirtRenderer = Shirt[i].GetComponent<Renderer>();
                    if (shirtRenderer != null)
                    {
                        // Check if the shirt is the one with the special condition (index 4)
                        if (i == 4 && shirtRenderer.materials.Length == 2 && sexcolor == 0)
                        {
                            ShirtMaterial = shirtRenderer.materials[0]; // Assign the second material
                        }
                        if (i == 4 && shirtRenderer.materials.Length == 3 && sexcolor == 1)
                        {
                            ShirtMaterial = shirtRenderer.materials[2]; // Assign the second material
                        }
                        else if (i == 5 && shirtRenderer.materials.Length > 1)
                        {
                            ShirtMaterial = shirtRenderer.materials[1]; // Assign the second material
                        }
                        else
                        {
                            ShirtMaterial = shirtRenderer.material; // Assign the first material
                        }
                    }
                }
            }
            selectedShirtIndex = index;
        }
        else
        ChangeShirtColor(6);
    }


    public void ChangeShirtColor(int colorIndex)
    {
        if (colorIndex >= 0 && colorIndex < HairColors.Length)
        {
            ShirtMaterial.color = HairColors[colorIndex];
            selectedShirtColorIndex = colorIndex;
        }
    }

    public void SelectPants(int index)
    {
        if (index >= 0 && index < Pants.Length)
        {
            for (int i = 0; i < Pants.Length; i++)
            {
                Pants[i].SetActive(i == index);
                if (i == index)
                {
                    Renderer pantsRenderer = Pants[i].GetComponent<Renderer>();
                    if (pantsRenderer != null)
                    {
                        if (i == 8 && pantsRenderer.materials.Length > 1 && sexcolor == 1)
                            {
                                PantsMaterial = pantsRenderer.materials[1]; // Assign the second material
                            }
                        
                        else
                        {
                            PantsMaterial = pantsRenderer.material;
                        }
                    }
                }
            }
            selectedPantsIndex = index;

        }
        else
        ChangePantsColor(6);
    }

    public void ChangePantsColor(int colorIndex)
    {
        if (colorIndex >= 0 && colorIndex < HairColors.Length)
        {
            PantsMaterial.color = HairColors[colorIndex];
            selectedPantsColorIndex = colorIndex;

        }
    }

    public void SelectShoes(int index)
    {
        if (index >= 0 && index < Shoes.Length)
        {
            for (int i = 0; i < Shoes.Length; i++)
            {
                Shoes[i].SetActive(i == index);
                if (i == index)
                {
                    Renderer shoesRenderer = Shoes[i].GetComponent<Renderer>();
                    if (shoesRenderer != null)
                    {
                        if (i == 2 && shoesRenderer.materials.Length > 1)
                        {
                            ShoesMaterial = shoesRenderer.materials[2]; // Assign the second material
                        }
                        else if (i == 1 && shoesRenderer.materials.Length > 1)
                        {
                            ShoesMaterial = shoesRenderer.materials[1]; // Assign the second material
                        }
                        else if (i == 3 && shoesRenderer.materials.Length > 1)
                        {
                            ShoesMaterial = shoesRenderer.materials[1]; // Assign the second material
                        }
                        else
                        {
                            ShoesMaterial = shoesRenderer.material; // Assign the first material
                        }
                    }
                }
            }
            selectedShoesIndex = index;

        }
        else
        ChangeShoesColor(6);
    }
    public void ChangeShoesColor(int colorIndex)
    {
        if (colorIndex >= 0 && colorIndex < HairColors.Length)
        {
            ShoesMaterial.color = HairColors[colorIndex];
            selectedShoesColorIndex = colorIndex;

        }
    }

    public void SelectFace(int index)
    {
        if (index >= 0 && index < Face.Length)
        {
            for (int i = 0; i < Face.Length; i++)
            {
                Face[i].SetActive(i == index);
            }
            selectedFaceIndex = index;

        }
    }
    public void SaveAvatar()
    {

        //////// Script Name AssetCharactor /////////


        // Save avatar data to the local list
        AssetCharactor.Instance.SaveAvatarData(this);

        // Optionally, you can save the data to disk or elsewhere here
    }

    public void BodySelection(int index)
    {
        selectedSex = index;
        ExitGames.Client.Photon.Hashtable customProperties = new ExitGames.Client.Photon.Hashtable();
        customProperties["selectedSex"] = selectedSex;
        PhotonNetwork.LocalPlayer.SetCustomProperties(customProperties);
        if(selectedSex == 3)
        {
            gender[0].SetActive(true);
            gender[1].SetActive(false);
            imagboyselect.ChangeButton_BoyAssets();
        }
        if(selectedSex == 4)
        {
            gender[0].SetActive(false);
            gender[1].SetActive(true);
            imagboyselect.ChangeButton_WomenAssets();
        }

    }

    [PunRPC]
    public void InitializeAvatar(int hairId, int hairColorId, int faceId, int faceColorId,
        int shirtId, int shirtColorId, int pantsId, int pantsColorId,
        int shoesId, int shoesColorId,
        int accessoryIds,
        int sexId, int skinID, string _Name)
    {
        // Set avatar data in AssetCharactor
        avatarData.Clear();
        avatarData.Add(new _AvatarData { Type = "Hair", Id = hairId, ColorId = hairColorId });
        avatarData.Add(new _AvatarData { Type = "Face", Id = faceId, ColorId = faceColorId });
        avatarData.Add(new _AvatarData { Type = "Shirt", Id = shirtId, ColorId = shirtColorId });
        avatarData.Add(new _AvatarData { Type = "Pants", Id = pantsId, ColorId = pantsColorId });
        avatarData.Add(new _AvatarData { Type = "Shoes", Id = shoesId, ColorId = shoesColorId });
        avatarData.Add(new _AvatarData { Type = "Accessory", Id = accessoryIds });
        //for (int i = 0; i < accessoryIds.Length; i++)
        //{
        //    avatarData.Add(new _AvatarData { Type = "Accessory", Id = accessoryIds[i]});
        //}

        avatarData.Add(new _AvatarData { Type = "SkinColor", Id = skinID, ColorId = 0 });
        avatarData.Add(new _AvatarData { Type = "Sex", Id = sexId, ColorId = 0 });

        Name = _Name;

        photonView.RPC("UpdatePlayerNameText", RpcTarget.OthersBuffered, _Name);
        Debug.Log($"InitializeAvatar called on client: Name={Name}, PlayerName.text={PlayerName.text}");
        // Apply the avatar data to the character
        LoadSavedAvatarData();
    }

}
