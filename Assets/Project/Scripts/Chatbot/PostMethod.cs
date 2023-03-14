using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using System.Collections.Generic;
using DG.Tweening;

public class PostMethod : MonoBehaviour
{
    private DroneUI droneUI;
    public GameObject drone;
    public float durationVal;
    public List<Button> buttons = new List<Button>();
    public List<Transform> locationPoints = new List<Transform>();
    TMP_InputField outputArea;
    TMP_InputField inputArea;
    private string username;
    private string text;

    void Start()
    {
        username = GameObject.Find("DisplayName").GetComponent<TextMeshPro>().text;
        outputArea = GameObject.Find("OutputArea").GetComponent<TMP_InputField>();
        inputArea = GameObject.Find("InputArea").GetComponent<TMP_InputField>();
        GameObject.Find("PostButton").GetComponent<Button>().onClick.AddListener(PostData);

        droneUI = FindObjectOfType<DroneUI>();
    
         var meetSalon = GameObject.Find("MeetSalon");
        var BowlingPlayground = GameObject.Find("BowlingPlayground");
        var ToggAraba = GameObject.Find("ToggAraba");
        var ShoeStore = GameObject.Find("ShoeStore");
        
        locationPoints.Add(meetSalon.transform);
        locationPoints.Add(BowlingPlayground.transform);
        locationPoints.Add(ToggAraba.transform);
        locationPoints.Add(ShoeStore.transform);
    }

    void PostData() => StartCoroutine(PostData_Coroutine());

    IEnumerator PostData_Coroutine()
    {
        Debug.Log("Username " + this.username + " " + outputArea.text);
        this.text = inputArea.text;
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
       
        Debug.Log(recieve[7]);

        if(recieve[7].Contains("meet")){
            
            StartCoroutine(goToLocationAfterSeconds(2));
            GoToLocation(locationPoints[0].position);
        }

        if(recieve[7].Contains("bowling") && recieve[7].Contains("enjoy")){
            
            Debug.Log("calisti");
            StartCoroutine(goToLocationAfterSeconds(2));
            droneUI.GoToLocation(locationPoints[1].position);
        }

        if(recieve[7].Contains("togg")){
            StartCoroutine(goToLocationAfterSeconds(2));
            droneUI.GoToLocation(locationPoints[2].position);
        }

        if(recieve[7].Contains("shoe")){
            StartCoroutine(goToLocationAfterSeconds(2));
            droneUI.GoToLocation(locationPoints[3].position);
        }

         return recieve[7]; 
    }

    private IEnumerator goToLocationAfterSeconds(float second){        
        yield return new WaitForSeconds(second);
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

     public void GoToLocation(Vector3 targetPosition)
    {
    float distance = Vector3.Distance(targetPosition, drone.transform.position);
    float duration = distance / durationVal; // adjust the speed by changing the value of the denominator

    // unparent the drone object
    drone.transform.parent = null;
    Debug.Log("gitti");
    // move the drone object to the target position using DoTween
    drone.transform.DOMove(new Vector3(targetPosition.x,drone.transform.position.y,targetPosition.z), duration).OnComplete(() => {
        // set the drone object as a child of the player's upper shoulder
        var Parent = GameObject.Find("DroneParent");
        drone.transform.SetParent(Parent.transform);
        Debug.Log("geldi");    
        // reset the position and rotation of the drone object
        drone.transform.localPosition = Parent.transform.localPosition;
        drone.transform.localRotation = Quaternion.identity;        
        
        if (distance > 0) {
            drone.transform.LookAt(new Vector3(targetPosition.x,drone.transform.position.y,targetPosition.z));
        }
    });
}


}