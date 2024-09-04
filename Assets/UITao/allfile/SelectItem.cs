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

    [Header("Selected Image")]
    public GameObject[] SelectImagesAccessory;
    public GameObject[] SelectImagesShirt;
    public GameObject[] SelectImagesPant;
    public GameObject[] SelectImagesShoes;
    public GameObject[] SelectImagesFace;
    public GameObject[] SelectImagesHair;

    [Header("Selected Color Image")]
    public GameObject[] SelectedImagesColorShirt;
    public GameObject[] SelectedImagesColorPant;
    public GameObject[] SelectedImagesColorHair;
    public GameObject[] SelectedImagesColorShoes;
    public GameObject[] SelectedImagesColorSkin;


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

    public bool _isSetup;
    public override void OnEnable()
    {

        if (photonView.IsMine)
        {
            if (!_isSetup)
            {
                if (_loginManager == null)
                {
                    _loginManager = FindObjectOfType<LoginManager>();
                }

                if (_loginManager._Avatar.name != "")
                {
                    // GET USER AVATAR
                    selectedHairIndex = _loginManager._Avatar.hair_id;
                    selectedHairColorIndex = _loginManager._Avatar.hair_color_id;
                    selectedShirtIndex = _loginManager._Avatar.shirt_id;
                    selectedShirtColorIndex = _loginManager._Avatar.shirt_color_id;
                    selectedPantsIndex = _loginManager._Avatar.pant_id;
                    selectedPantsColorIndex = _loginManager._Avatar.pant_color_id;
                    selectedShoesIndex = _loginManager._Avatar.shoe_id;
                    selectedShoesColorIndex = _loginManager._Avatar.shoe_color_id;
                    selectedFaceIndex = _loginManager._Avatar.face_id;
                    selectedAccessoryIndex = _loginManager._Avatar.accessory_id;
                    selectedSkinColor = _loginManager._Avatar.skin_id;
                    //selectedSex = _loginManager._Avatar.gender_id; - FIXED BY PREFAB

                    // GET NAME
                    Name = _loginManager._Avatar.name;
                    BodySelection(selectedSex);
                }

                _isSetup = true;
            }
            
            
            
        }

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

        // 
        BodySelection(selectedSex);

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
        
        
    }

    private void Update()
    {
        if (_loginManager != null && isInitialized == false)
        {
            isInitialized = true;
           
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

    public SkinnedMeshRenderer _Body;

    public void ChangeSkinColor(int colorIndex)
    {



        //if (Skinbody == null)
        //{
            Skinbody = _Body.material;
        //}

        if (colorIndex >= 0 && colorIndex < ColorsBody.Length)
        {
            Skinbody.color = ColorsBody[colorIndex];
            selectedSkinColor = colorIndex;
        }

        if (imagboyselect != null)
        {
            foreach (GameObject x in SelectedImagesColorSkin)
            {
                x.SetActive(false);
            }
            if (colorIndex >= 0 && colorIndex < ColorsBody.Length)
            {

                if (colorIndex == selectedSkinColor)
                {
                    SelectedImagesColorSkin[colorIndex].SetActive(true);
                }
                else
                {
                    SelectedImagesColorSkin[colorIndex].SetActive(false);

                }
            }
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
        if (imagboyselect != null)
        {
            foreach (GameObject x in SelectImagesHair)
            {
                x.SetActive(false);
            }

            if (index >= 0 && index < SelectImagesHair.Length)
            {
                Debug.Log("Select Hair = " + index);
                if (index == selectedHairIndex)
                {
                    SelectImagesHair[index].SetActive(true);
                }
                else
                {
                    SelectImagesHair[index].SetActive(false);

                }
            }
        }
    }

    public void ChangeHairColor(int colorIndex)
    {


        if (colorIndex >= 0 && colorIndex < HairColors.Length)
        {
            if (!HairMaterial)
            {
                 SelectHair(selectedHairIndex);
                 HairMaterial.color = HairColors[colorIndex];
            }
            else
            {
                HairMaterial.color = HairColors[colorIndex];
            }

            
            selectedHairColorIndex = colorIndex;
        }
        if (imagboyselect != null)
        {
            foreach (GameObject x in SelectedImagesColorHair)
            {
                x.SetActive(false);
            }

            if (colorIndex >= 0 && colorIndex < HairColors.Length)
            {
                //Debug.Log("Select Hair = " + colorIndex);
                if (colorIndex == selectedHairColorIndex)
                {
                    SelectedImagesColorHair[colorIndex].SetActive(true);
                }
                else
                {
                    SelectedImagesColorHair[colorIndex].SetActive(false);

                }
            }
        }
    }

    public void ToggleAccessory(int index)
    {
        foreach(GameObject x in Accessories)
        {
            x.SetActive(false);
        }

        foreach (GameObject x in SelectImagesAccessory)
        {
            x.SetActive(false);
        }



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
        if (imagboyselect != null)
        {
            foreach (GameObject x in SelectImagesShirt)
            {
                x.SetActive(false);
            }

            if (index >= 0 && index < SelectImagesShirt.Length)
            {
                Debug.Log("Select Hair = " + index);
                if (index == selectedShirtIndex)
                {
                    SelectImagesShirt[index].SetActive(true);
                }
                else
                {
                    SelectImagesShirt[index].SetActive(false);

                }
            }
        }
    }


    public void ChangeShirtColor(int colorIndex)
    {


        if (colorIndex >= 0 && colorIndex < HairColors.Length)
        {
            if (!ShirtMaterial)
            {
                SelectShirt(selectedShirtIndex);
                ShirtMaterial.color = HairColors[colorIndex];           
            }
            else
            {
                ShirtMaterial.color = HairColors[colorIndex];
            }
            
            selectedShirtColorIndex = colorIndex;
        }
        if (imagboyselect != null)
        {
            foreach (GameObject x in SelectedImagesColorShirt)
            {
                x.SetActive(false);
            }

            if (colorIndex >= 0 && colorIndex < HairColors.Length)
            {

                //Debug.Log($"selected Shirt Color = {selectedShirtIndex}");
                if (colorIndex == selectedShirtColorIndex)
                {
                    SelectedImagesColorShirt[colorIndex].SetActive(true);
                }
                else
                {
                    SelectedImagesColorShirt[colorIndex].SetActive(false);

                }
            }
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

        if (imagboyselect != null)
        {
            foreach (GameObject x in SelectImagesPant)
            {
                x.SetActive(false);
            }
            if (index >= 0 && index < SelectImagesPant.Length)
            {

                if (index == selectedPantsIndex)
                {
                    SelectImagesPant[index].SetActive(true);
                }
                else
                {
                    SelectImagesPant[index].SetActive(true);
                }
            }
        }
    }

    public void ChangePantsColor(int colorIndex)
    {


        if (colorIndex >= 0 && colorIndex < HairColors.Length)
        {
            if (!PantsMaterial)
            {
                SelectPants(selectedPantsIndex);
                PantsMaterial.color = HairColors[colorIndex];
            }
            else
            {
                PantsMaterial.color = HairColors[colorIndex];
            }

            
            selectedPantsColorIndex = colorIndex;

        }

        if (imagboyselect != null)
        {
            foreach (GameObject x in SelectedImagesColorPant)
            {
                x.SetActive(false);
            }
            if (colorIndex >= 0 && colorIndex < HairColors.Length)
            {

                //Debug.Log($"selected Shirt Color = {selectedShirtIndex}");
                if (colorIndex == selectedPantsColorIndex)
                {
                    SelectedImagesColorPant[colorIndex].SetActive(true);
                }
                else
                {
                    SelectedImagesColorPant[colorIndex].SetActive(false);

                }
            }
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

        if (imagboyselect != null)
        {
            foreach (GameObject x in SelectImagesShoes)
            {
                x.SetActive(false);
            }
            if (index >= 0 && index < SelectImagesShoes.Length)
            {

                if (index == selectedShoesIndex)
                {
                    SelectImagesShoes[index].SetActive(true);
                }
                else
                {
                    SelectImagesShoes[index].SetActive(true);
                }
            }
        }
    }
    public void ChangeShoesColor(int colorIndex)
    {

        if (colorIndex >= 0 && colorIndex < HairColors.Length)
        {
            if (!ShoesMaterial)
            {
                SelectShoes(selectedPantsIndex);
                ShoesMaterial.color = HairColors[colorIndex];
            }
            else
            {
                ShoesMaterial.color = HairColors[colorIndex];
            }

            
            selectedShoesColorIndex = colorIndex;

        }
        if (imagboyselect != null)
        {
            foreach (GameObject x in SelectedImagesColorShoes)
            {
                x.SetActive(false);
            }
            if (colorIndex >= 0 && colorIndex < HairColors.Length)
            {

                //Debug.Log($"selected Shirt Color = {selectedShirtIndex}");
                if (colorIndex == selectedShoesColorIndex)
                {
                    SelectedImagesColorShoes[colorIndex].SetActive(true);
                }
                else
                {
                    SelectedImagesColorShoes[colorIndex].SetActive(false);
                }
            }
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
        if (imagboyselect != null)
        {
            foreach (GameObject x in SelectImagesFace)
            {
                x.SetActive(false);
            }


            if (index >= 0 && index < SelectImagesFace.Length)
            {
                Debug.Log("Select Face = " + index);

                if (index == selectedFaceIndex)
                {
                    SelectImagesFace[index].SetActive(true);
                }
                else
                {
                    SelectImagesFace[index].SetActive(false);
                }
            }
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

        // Whatever the hell it is, don't mess with it.
        // SELECT BODY
        selectedSex = index;
        if(selectedSex == 3)
        {
            if (gender[0])
                gender[0].SetActive(true);
            if(gender[1])
                gender[1].SetActive(false);

            if(imagboyselect)
                imagboyselect.ChangeButton_BoyAssets();
        }
        if(selectedSex == 4)
        {
            if (gender[0])
                gender[0].SetActive(false);
            if (gender[1])
                gender[1].SetActive(true);

            if (imagboyselect)
                imagboyselect.ChangeButton_WomenAssets();
        }

        // SETUP AVATAR 
        _SetUpAvatar();

    }

    public void _SetUpAvatar()
    {
        if(PlayerName)
            PlayerName.text = Name;

        SelectHair(selectedHairIndex);
        SelectShirt(selectedShirtIndex);
        SelectPants(selectedPantsIndex);
        SelectShoes(selectedShoesIndex);
        SelectFace(selectedFaceIndex);
        ChangeHairColor(selectedHairColorIndex);
        ChangeSkinColor(selectedSkinColor);
        ChangeShirtColor(selectedShirtColorIndex);
        ChangePantsColor(selectedPantsColorIndex);
        ChangeShoesColor(selectedShoesColorIndex);
        ToggleAccessory(selectedAccessoryIndex);

        // SENT TO OTHER
        photonView.RPC("InitializeAvatar", RpcTarget.AllBuffered,
        selectedHairIndex, selectedHairColorIndex, selectedFaceIndex,
        selectedShirtIndex, selectedShirtColorIndex, selectedPantsIndex, selectedPantsColorIndex,
        selectedShoesIndex, selectedShoesColorIndex, selectedAccessoryIndex,
        selectedSex, selectedSkinColor, Name);
        // SENT NAME
        photonView.RPC("UpdatePlayerNameText", RpcTarget.OthersBuffered, Name);

        Debug.Log("DONE SETUP");
    }

    [PunRPC]
    public void InitializeAvatar(int hairId, int hairColorId, int faceId,
        int shirtId, int shirtColorId, int pantsId, int pantsColorId,
        int shoesId, int shoesColorId,
        int accessoryIds,
        int sexId, int skinID, string _Name)
    {
        Name = _Name;
        PlayerName.text = _Name;

        Debug.Log($"InitializeAvatar called on client: Name={_Name}, PlayerName.text={PlayerName.text}");

        ChangeSkinColor(skinID);
        SelectHair(hairId);
        ChangeHairColor(hairColorId);
        SelectFace(faceId);
        SelectShirt(shirtId);
        ChangeShirtColor(shirtColorId);
        SelectPants(pantsId);
        ChangePantsColor(pantsColorId);
        SelectShoes(shoesId);
        ChangeShoesColor(shoesColorId);
        ToggleAccessory(accessoryIds);
    }

}
