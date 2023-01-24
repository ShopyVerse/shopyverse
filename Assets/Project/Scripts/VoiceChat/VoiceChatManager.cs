using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VoiceChatManager : MonoBehaviour
{

#if UNITY_WEBGL && !UNITY_EDITOR
    AgoraWebChat agoraWebChat;
#else
    // Fill in your app ID.
    [SerializeField] string _appID = "23cb8370880e4e819e1e5f4e24efa012"; //896ce96e28204f32a61e3dbb2aa74b22 
    // Fill in your channel name.
    [SerializeField] string _channelName;
    // Fill in the temporary token you obtained from Agora Console.
    [SerializeField] private string serverUrl = "https://agora-token-service-production-d9be.up.railway.app"; // The base URL to your token server. For example, https://app-name.up.railway.app"
    [SerializeField] private int tokenExpireTime = 7200; //Expire time in Seconds.
    [SerializeField] private string uid = "1"; // An integer that identifies the user.        


    AgoraVoiceChat agoraVoiceChat;
#endif       
    private void Start()
    {
#if UNITY_WEBGL && !UNITY_EDITOR

        agoraWebChat = this.gameObject.AddComponent<AgoraWebChat>();
#else
        agoraVoiceChat = this.gameObject.AddComponent<AgoraVoiceChat>();
        agoraVoiceChat.SetupVoiceSDKEngine(_appID);
#endif
    }

    public void Join()
    {
#if UNITY_WEBGL && !UNITY_EDITOR
        agoraWebChat.JoinChat();
#else
        agoraVoiceChat.JoinWithFetchedToken(serverUrl, _channelName, uid, tokenExpireTime);
#endif
    }

    public void Leave()
    {
#if UNITY_WEBGL && !UNITY_EDITOR
        Debug.Log("Leave will added");
#else
        agoraVoiceChat.Leave();
#endif
    }

    public void Mute(bool value)
    {
#if UNITY_WEBGL && !UNITY_EDITOR
        agoraWebChat.SetMuteState (value);
#else
        agoraVoiceChat.MuteRemoteAudio(value);
#endif
    }

    public void FetchToken()
    {
#if UNITY_WEBGL && !UNITY_EDITOR
        agoraWebChat.FetchToken();
#else
        agoraVoiceChat.FetchTokenButton(serverUrl, _channelName, uid, tokenExpireTime);
#endif
    }   
}
