using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ItemData", menuName = "ScriptableObjects/ItemData", order = 1)]
public class ItemData : ScriptableObject
{
    public GameObject[] Hair; // Array of hair GameObjects
    public Material HairMaterial; // Single material applied to active hair
    public Color[] HairColors; // Array of colors to apply to hair
    public GameObject[] Accessories;
    public GameObject[] Shirts;
    public GameObject[] Pants;
}
