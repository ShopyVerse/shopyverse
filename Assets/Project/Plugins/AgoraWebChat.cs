using UnityEngine;
using System.Runtime.InteropServices;
using System.Collections;
using TMPro;

public class AgoraWebChat : MonoBehaviour
{
    [DllImport("__Internal")]
    public static extern void JoinChannel();

    [DllImport("__Internal")]
    public static extern void ChangeMuteState(bool state);

    [DllImport("__Internal")]
    public static extern void FetchTokenInternal();

    public void JoinChat()
    {
        Debug.LogWarning("AgoraChat.JoinChannel");
        JoinChannel();
    }

    public void SetMuteState(bool state)
    {
        ChangeMuteState(state);
    }

    public void FetchToken(){
        FetchTokenInternal();
    }
}