using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using TMPro;

public class ChatAll : MonoBehaviour
{
    public TMP_InputField inputField;
    public GameObject messagePrefab;
    public Transform contentTransform;
    public int maxMessages = 8; // Maximum number of messages to keep in the chat

    private Queue<GameObject> messageQueue = new Queue<GameObject>();
    private string playerNickname;

    // List of profane words (this is just an example, you may want to use a more comprehensive list)
    private List<string> profaneWords = new List<string>
    { 
        // คำหยาบภาษาไทย
        "ไอ้", "อี", "เหี้ย", "เชี่ย", "แม่ง", "สัส", 
        "สัด", "มึง", "กู", "ห่า", "ควย", 
        "เย็ด", "หี", "แตด", "ฟาย", "ตอแหล", "ระยำ", 
        "สัตว์", "ดอกทอง", "จัญไร", "ชาติชั่ว", "เห้", 
        "พ่อง", "แม่ม", "สถุน", "กาก", "สวะ", "หน้าตัวเมีย",

        // คำหยาบภาษาอังกฤษ
        "fuck", "shit", "bitch", "asshole", "bastard", "dick", 
        "pussy", "cunt", "motherfucker", "nigger", "fag", 
        "slut", "whore", "douche", "cock", "jerk", "freak", 
        "suck", "crap", "dumbass", "retard", "idiot", "moron",

        // รูปแบบ Leet ภาษาไทย
        "คuย", "ค_ย", "ควe", "ค.วย",
        "hี", "ฮี",
        "เ_้ย", "เ_ี้ย", "เ-้ย", "เ-ี้ย",
        "สaส", "สั_ส", "สัต", "สัตว์",
        "เ_ด", "เ-ด", "เ_ย็ด", "เย็d",

        // รูปแบบ Leet ภาษาอังกฤษ
        "f@ck", "sh!t", "b!tch", "4ss", "b@stard", "d!ck", 
        "p@ssy", "c*nt", "m0therfucker", "n!gger", "f@g", 
        "sl*t", "wh0re", "d0uche", "c*ck", "j3rk", "fr3ak", 
        "suck", "cr@p", "dumb@ss", "r3tard", "idi0t", "m0r0n"
    }; 
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            SendMessage();
        }
    }

    public void SendMessage()
    {
        string messageText = inputField.text;

        if (string.IsNullOrWhiteSpace(messageText)) return;

        // Replace profane words with ****
        messageText = ReplaceProfaneWords(messageText);

        playerNickname = PhotonNetwork.LocalPlayer.NickName;
        if (string.IsNullOrEmpty(playerNickname))
        {
            playerNickname = "UnknownPlayer"; // Default nickname if none is set
        }

        GetComponent<PhotonView>().RPC("GetMessage", RpcTarget.All, messageText, playerNickname);
        inputField.text = ""; // Clear input field after sending
    }

    private string ReplaceProfaneWords(string message)
    {
        foreach (var word in profaneWords)
        {
            string replacement = new string('*', word.Length);
            message = message.Replace(word, replacement, System.StringComparison.OrdinalIgnoreCase);
        }
        return message;
    }

    [PunRPC]
    public void GetMessage(string receiveMessage, string nickname)
    {
        GameObject messageObject = Instantiate(messagePrefab, contentTransform);
        Message messageComponent = messageObject.GetComponent<Message>();
        
        if (messageComponent != null)
        {
            messageComponent.MyMessage.text = $"{nickname}: {receiveMessage}";

            // Add new message to the queue
            messageQueue.Enqueue(messageObject);

            // Remove the oldest message if the number of messages exceeds maxMessages
            if (messageQueue.Count > maxMessages)
            {
                GameObject oldestMessage = messageQueue.Dequeue();
                Destroy(oldestMessage); // Remove oldest message from the chat
            }

            // Optional: Scroll content to bottom if you are using a ScrollRect
            ScrollToBottom();
        }
        else
        {
            Debug.LogError("Message component not found on the prefab.");
        }
    }

    private void ScrollToBottom()
    {
        // Implement scrolling logic if using ScrollRect
        // ScrollRect scrollRect = contentTransform.GetComponent<ScrollRect>();
        // if (scrollRect != null)
        // {
        //     scrollRect.verticalNormalizedPosition = 0f;
        // }
    }
}
