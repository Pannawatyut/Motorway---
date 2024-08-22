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

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            SendMessage();
        }
    }

    public void SendMessage()
    {
        if (string.IsNullOrWhiteSpace(inputField.text)) return;

        playerNickname = PhotonNetwork.LocalPlayer.NickName;
        if (string.IsNullOrEmpty(playerNickname))
        {
            playerNickname = "UnknownPlayer"; // Default nickname if none is set
        }

        GetComponent<PhotonView>().RPC("GetMessage", RpcTarget.All, inputField.text, playerNickname);
        inputField.text = ""; // Clear input field after sending
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
