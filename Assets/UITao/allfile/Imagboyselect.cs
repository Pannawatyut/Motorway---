using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Imagboyselect : MonoBehaviour
{
    public ImageBoy BoyAssets;
    public List<Image> Hairbutton;
    public List<Image> ShirtButton;
    public List<Image> PantsButton;
    public List<Image> ShoesButton;
    public List<Image> Accessory;

    public ImageWomen WomenAssets;

    public GameObject[] Imagegendr1;
    public GameObject buttondisable;
    public void ChangeButton_BoyAssets()
    {
        // Check if Imagegendr1 has the elements before accessing them
        if (Imagegendr1.Length > 0 && Imagegendr1[0] != null)
        {
            Imagegendr1[0].SetActive(true);
        }
        if (Imagegendr1.Length > 1 && Imagegendr1[1] != null)
        {
            Imagegendr1[1].SetActive(false);
        }

        // Update hair buttons
        for (int i = 0; i < Hairbutton.Count; i++)
        {
            if (i < BoyAssets.hair.Length)
            {
                if (Hairbutton[i] != null)
                {
                    Hairbutton[i].sprite = BoyAssets.hair[i];
                }
            }
        }

        // Update shirt buttons
        for (int i = 0; i < ShirtButton.Count; i++)
        {
            if (i < BoyAssets.Shirts.Length)
            {
                if (ShirtButton[i] != null)
                {
                    ShirtButton[i].sprite = BoyAssets.Shirts[i];
                }
            }
        }

        // Update pants buttons
        for (int i = 0; i < PantsButton.Count; i++)
        {
            if (i < BoyAssets.Pants.Length)
            {
                if (PantsButton[i] != null)
                {
                    PantsButton[i].sprite = BoyAssets.Pants[i];
                }
            }
        }

        // Update shoes buttons
        for (int i = 0; i < ShoesButton.Count; i++)
        {
            if (i < BoyAssets.Shoes.Length)
            {
                if (ShoesButton[i] != null)
                {
                    ShoesButton[i].sprite = BoyAssets.Shoes[i];
                }
            }
        }

        // Update accessory buttons
        for (int i = 0; i < Accessory.Count; i++)
        {
            if (i < BoyAssets.Accessories.Length)
            {
                if (Accessory[i] != null)
                {
                    Accessory[i].sprite = BoyAssets.Accessories[i];
                }
            }
        }
    }

    public void ChangeButton_WomenAssets()
    {
        // Check if Imagegendr1 has the elements before accessing them
        if (Imagegendr1.Length > 0 && Imagegendr1[1] != null)
        {
            Imagegendr1[1].SetActive(true);
        }
        if (Imagegendr1.Length > 1 && Imagegendr1[0] != null)
        {
            Imagegendr1[0].SetActive(false);
        }

        // Update hair buttons
        for (int i = 0; i < Hairbutton.Count; i++)
        {
            if (i < WomenAssets.hair.Length)
            {
                if (Hairbutton[i] != null)
                {
                    Hairbutton[i].sprite = WomenAssets.hair[i];
                }
            }
        }
        //// Specifically disable the 6th hair button (index 5)
        //if (Hairbutton.Count > 5 && Hairbutton[5] != null)
        //{
        //    Hairbutton[5].gameObject.SetActive(false); // Disable Hairbutton[5]
        //}
        // Update shirt buttons
        for (int i = 0; i < ShirtButton.Count; i++)
        {
            if (i < WomenAssets.Shirts.Length)
            {
                if (ShirtButton[i] != null)
                {
                    ShirtButton[i].sprite = WomenAssets.Shirts[i];
                }
            }
        }

        // Update pants buttons
        for (int i = 0; i < PantsButton.Count; i++)
        {
            if (i < WomenAssets.Pants.Length)
            {
                if (PantsButton[i] != null)
                {
                    PantsButton[i].sprite = WomenAssets.Pants[i];
                }
            }
        }

        // Update shoes buttons
        for (int i = 0; i < ShoesButton.Count; i++)
        {
            if (i < WomenAssets.Shoes.Length)
            {
                if (ShoesButton[i] != null)
                {
                    ShoesButton[i].sprite = WomenAssets.Shoes[i];
                }
            }
        }

        // Update accessory buttons
        for (int i = 0; i < Accessory.Count; i++)
        {
            if (i < WomenAssets.Accessories.Length)
            {
                if (Accessory[i] != null)
                {
                    Accessory[i].sprite = WomenAssets.Accessories[i];
                }
            }
        }
    }

    public void UpdateButtonsBySex(int selectedSex)
    {
        Debug.Log("<color=green>Start</color>");
        switch (selectedSex)
        {
            case 0:
                ChangeButton_BoyAssets();
                buttondisable.SetActive(true);
                break;
            case 1:
                ChangeButton_WomenAssets();
                buttondisable.SetActive(false);
                break;
        }
    }
}
