using System.Collections.Generic;
using UnityEngine;

public class AvatarDataHandler : MonoBehaviour
{
    public List<SelectItem._AvatarData> AvatarData;

    void Start()
    {
        // Initialize AvatarData list
        AvatarData = new List<SelectItem._AvatarData>();

        // Load saved avatar data
        LoadAvatarData();
    }

    public void SaveAvatarData(SelectItem selectItem)
    {
        AvatarData.Clear();

        // Add avatar data from SelectItem
        AvatarData.Add(new SelectItem._AvatarData { Type = "Hair", Id = selectItem.selectedHairIndex, ColorId = selectItem.selectedHairColorIndex });
        AvatarData.Add(new SelectItem._AvatarData { Type = "Shirt", Id = selectItem.selectedShirtIndex, ColorId = selectItem.selectedShirtColorIndex });
        AvatarData.Add(new SelectItem._AvatarData { Type = "Pants", Id = selectItem.selectedPantsIndex, ColorId = selectItem.selectedPantsColorIndex });
        AvatarData.Add(new SelectItem._AvatarData { Type = "Shoes", Id = selectItem.selectedShoesIndex, ColorId = selectItem.selectedShoesColorIndex });
        AvatarData.Add(new SelectItem._AvatarData { Type = "Face", Id = selectItem.selectedFaceIndex, ColorId = 0 });  // Assuming no color for face
        AvatarData.Add(new SelectItem._AvatarData { Type = "Accessory", Id = selectItem.selectedAccessoryIndex, ColorId = 0 });  // Assuming no color for accessory

        // Save avatar data
        SaveAvatarData();
    }

    private void LoadAvatarData()
    {
        // Load avatar data
        AvatarData = LoadSavedAvatarData();

        // Apply loaded avatar data to SelectItem
        SelectItem selectItem = GetComponent<SelectItem>(); // Assuming SelectItem is attached to the same GameObject
        if (selectItem != null)
        {
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
                    case "Accessory":
                        selectItem.ToggleAccessory(data.Id);
                        break;
                }
            }
        }
        else
        {
            Debug.LogError("SelectItem component not found on the GameObject.");
        }
    }

    private void SaveAvatarData()
    {

    }

    private List<SelectItem._AvatarData> LoadSavedAvatarData()
    {
        return new List<SelectItem._AvatarData>(); 
    }
}
