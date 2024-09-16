using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class NPC_APICounter : MonoBehaviour
{
    public static NPC_APICounter Instance { get; private set; }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }
        DontDestroyOnLoad(gameObject);

        //Test();

        // NPC_APICounter.Instance._API_Caller_NPC(_id)
    }

    

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            _API_Caller_NPC(1);
        }
    }

    public void _API_Caller_NPC(int _id)
    {
        StartCoroutine(_NPC_Interactive(_id));
    }

    public class _NPC
    {
        public int npc_id;
    }

    IEnumerator _NPC_Interactive(int _id)
    {
        _NPC _tester = new _NPC();
        _tester.npc_id = _id;

        string json = JsonUtility.ToJson(_tester);

        using var request = new UnityWebRequest(LoginManager.Instance._APIURL + "/api/user/talkNpc", "POST")
        {
            uploadHandler = new UploadHandlerRaw(System.Text.Encoding.UTF8.GetBytes(json)),
            downloadHandler = new DownloadHandlerBuffer()
        };

        request.SetRequestHeader("Content-Type", "application/json");
        request.SetRequestHeader("Authorization", LoginManager.Instance._Account.access_token);

        yield return request.SendWebRequest();

        if (request.result != UnityWebRequest.Result.Success)
        {
            Debug.Log("Talk NPC : FAILED");
        }
        else
        {
            Debug.Log("Talk NPC : OK");
        }
    }
}
