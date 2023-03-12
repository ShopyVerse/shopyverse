using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using System.Collections.Generic;

public class PostMethod : MonoBehaviour
{
    TMP_InputField outputArea;
    private string username;
    private string text;

    void Start()
    {
        username = GameObject.Find("DisplayName").GetComponent<TextMeshPro>().text;
        outputArea = GameObject.Find("OutputArea").GetComponent<TMP_InputField>();
        GameObject.Find("PostButton").GetComponent<Button>().onClick.AddListener(PostData);
    }

    void PostData() => StartCoroutine(PostData_Coroutine());

    IEnumerator PostData_Coroutine()
    {
        Debug.Log("Username " + this.username + " " + outputArea.text);
        this.text = outputArea.text;
        string uri = "https://shopyverse-backend.herokuapp.com/rasatext";
      
        var dataToPost = new PostedData() { username = this.username, text = this.text };

        var postRequest = CreateRequest(uri, RequestType.POST, dataToPost);
        yield return postRequest.SendWebRequest();

        Debug.Log("PostData: " + postRequest.downloadHandler.text);
        outputArea.text = Decode(postRequest.downloadHandler.text);
        Debug.Log("PostData binary : " + postRequest.downloadHandler.data);


       // var deserializedPostData = JsonUtility.FromJson<PostResult>("{\"root\":" + postRequest.downloadHandler.text + "}");
       // Debug.Log("deserializedPostData: " + deserializedPostData.text);
       // outputArea.text = deserializedPostData.text;
    }

    string Decode(string received)
    {
        string[] recieve = received.Split("\"");
        return recieve[7];
    }

    private UnityWebRequest CreateRequest(string path, RequestType type = RequestType.GET, object data = null)
    {
        var request = new UnityWebRequest(path, type.ToString());

        if (data != null)
        {
            var bodyRaw = System.Text.Encoding.UTF8.GetBytes(JsonUtility.ToJson(data));
            request.uploadHandler = new UploadHandlerRaw(bodyRaw);

        }

        request.downloadHandler = new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");


        return request;
    }

    public enum RequestType
    {
        GET = 0,
        POST = 1,
        PUT = 2
    }

    [System.Serializable]
    public class PostResult
    {
        public string recipient_id { get; set; }
        public string text { get; set; }
    }

    [System.Serializable]
    public class PostedData
    {
        public string username;
        public string text;
    }

}