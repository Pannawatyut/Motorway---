using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "PlayerCharacter")]
public class PlayerCharacter : ScriptableObject
{
    [System.Serializable]
    public class AvatarData
    {
        public string Type;
        public int Id;
        public int ColorId;
    }
    public AvatarData[] avatarData;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }
}
