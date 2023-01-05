using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.SceneManagement;
public class PhotonManager : MonoBehaviourPunCallbacks
{
    public static PhotonManager _networkManager;
    [SerializeField] string roomName;
    [SerializeField] GameObject pcObject;
    [SerializeField] GameObject vrObject;

    [SerializeField] GameObject game;
    [SerializeField] GameObject login;

    [SerializeField] VoiceChatManager agoraChat;

    private void Awake()
    {
#if UNITY_WEBGL
            vrObject.SetActive(false);
            pcObject.SetActive(true);     
#endif

#if UNITY_ANDROID
        // pcObject.SetActive(false); for vr
        // vrObject.SetActive(true);
         vrObject.SetActive(false); //for mobile
        pcObject.SetActive(true);     
#endif
    }
    void Start()
    {
        if (_networkManager == null)
        {
            _networkManager = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
          PhotonNetwork.Destroy(gameObject);
        }
        PhotonNetwork.ConnectUsingSettings();

    }

    public override void OnConnectedToMaster()
    {
        DebugConsole("ConnectedToServer");
        PhotonNetwork.JoinLobby();
    }
    public override void OnJoinedLobby()
    {
        DebugConsole("JoinLobby");
    }
    public override void OnJoinedRoom()
    {
        DebugConsole("Odadayiz");

        login.SetActive(false);
        game.SetActive(true);

        agoraChat.Join();
        
        CreatePlayer();
        
    }
    private void ChangeScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }
    private void CreateRoomSync(RoomOptions roomOptions)
    {
        PhotonNetwork.CreateRoom(roomName, roomOptions, TypedLobby.Default);
    }
    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        RoomOptions roomOptions = new RoomOptions
        {
            MaxPlayers = 20,
            IsOpen = true,
            IsVisible = true
        };
        CreateRoomSync(roomOptions);
    }
    IEnumerator JoinRoom()
    {
        //ChangeScene("Environment_2");
        yield return null;
        PhotonNetwork.JoinRoom(roomName);
    }

    public void Login()
    {
        if (PhotonNetwork.InLobby)
        {
            DebugConsole("Waiting...");
            StartCoroutine(JoinRoom());
        }

    }
    public void CreatePlayer()
    {
        string gender = PlayerPrefs.GetString("Gender");
        Debug.Log(gender + "qqq");
        GameObject[] points = GameObject.FindGameObjectsWithTag("SpawnPoint");
        int value = Random.Range(0, points.Length);

#if UNITY_WEBGL

            if (gender != null)
            {
                PhotonNetwork.Instantiate(gender, points[value].transform.position, Quaternion.identity);
                return;
            }
            else
            {
                Debug.Log(gender + "qqq");
            }
#elif UNITY_ANDROID

            //Vector3 positionForVr = new Vector3(points[value].transform.position.x, points[value].transform.position.y - 0.4f, points[value].transform.position.z);
           // PhotonNetwork.Instantiate(PlayerPrefs.GetString("GenderVr"), positionForVr, Quaternion.identity);
             if (gender != null)
            {
                PhotonNetwork.Instantiate(gender, points[value].transform.position, Quaternion.identity);
                return;
            }
            else
            {
                Debug.Log(gender + "qqq");
            }
#endif 
    }

    public void DebugConsole(string status)
    {
        Debug.Log(status);
    }
}