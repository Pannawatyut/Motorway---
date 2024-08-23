using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public class AssetCharactor : MonoBehaviourPunCallbacks
{
    public static AssetCharactor Instance { get; private set; }

    [SerializeField]
    private List<SelectItem._AvatarData> avatarData = new List<SelectItem._AvatarData>();


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

}