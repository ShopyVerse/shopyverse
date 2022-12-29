using System;
using System.Collections;
using System.Collections.Generic;
using Agora.Rtc;
using Photon.Pun;
using Photon.Realtime;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

using Hashtable = ExitGames.Client.Photon.Hashtable;

public class AgoraVoiceChat : MonoBehaviour
{
    internal IRtcEngine RtcEngine;

    private long remoteUid;

    string fetchedToken;

    void OnApplicationQuit()
    {
        if (RtcEngine != null)
        {
            Leave();
            RtcEngine.Dispose();
            RtcEngine = null;
        }
    }

    public void SetupVoiceSDKEngine(string _appID)
    {
        // Create an RtcEngine instance.
        RtcEngine = Agora.Rtc.RtcEngine.CreateAgoraRtcEngine();
        RtcEngineContext context =
            new RtcEngineContext(_appID,
                0,
                true,
                CHANNEL_PROFILE_TYPE.CHANNEL_PROFILE_LIVE_BROADCASTING,
                AUDIO_SCENARIO_TYPE.AUDIO_SCENARIO_DEFAULT);

        // Initialize RtcEngine.
        RtcEngine.Initialize (context);

        InitEventHandler();
    }

    private void InitEventHandler()
    {
        // Creates a UserEventHandler instance.
        UserEventHandler handler = new UserEventHandler(this);
        RtcEngine.InitEventHandler (handler);
    }

    //
    internal class UserEventHandler : IRtcEngineEventHandler
    {
        private readonly AgoraVoiceChat _audioSample;

        internal UserEventHandler(AgoraVoiceChat audioSample)
        {
            _audioSample = audioSample;
        }

        public override void OnUserJoined(
            RtcConnection connection,
            uint uid,
            int elapsed
        )
        {
            // Save the remote user ID in a variable.
            _audioSample.remoteUid = uid;
        }

        // This callback is triggered when the local user joins the channel.
        public override void OnJoinChannelSuccess(
            RtcConnection connection,
            int elapsed
        )
        {
            Debug.Log("join channel success: " + connection.channelId);
            Hashtable hash = new Hashtable();
            hash.Add("agoraID", connection.localUid.ToString());
            PhotonNetwork.SetPlayerCustomProperties (hash);
        }

        public override void OnError(int a, string b)
        {
            Debug.Log("error code" + a);
            Debug.Log("error msg" + b);
        }

        public virtual void OnConnectionStateChanged(
            CONNECTION_STATE_TYPE state,
            CONNECTION_CHANGED_REASON_TYPE reason
        )
        {
            Debug.Log (state);
            Debug.Log (reason);
        }
    }

    public void MuteRemoteAudio(bool value)
    {
        // Pass the uid of the remote user you want to mute.
        RtcEngine.MuteLocalAudioStream (value);
        //RtcEngine.MuteRemoteAudioStream(System.Convert.ToUInt32(remoteUid), value);
    }

    IEnumerator
    FetchTokenForJoin(string url, string channel, string userId, int TimeToLive) /* , Action<string> callback = null */
    {
        int role = 1;
        UnityWebRequest request =
            UnityWebRequest
                .Get(string
                    .Format("{0}/rtc/{1}/{2}/uid/{3}/?expiry={4}",
                    url,
                    channel,
                    role,
                    userId,
                    TimeToLive));

        yield return request.SendWebRequest();
        if (request.isNetworkError || request.isHttpError)
        {
            Debug.Log(request.error);

            /* callback(null); */
            yield break;
        }
        AgoraToken tokenInfo =
            JsonUtility.FromJson<AgoraToken>(request.downloadHandler.text);

        /* callback(tokenInfo.rtcToken); */
        fetchedToken = tokenInfo.rtcToken; // alternatif çözüm olarak statik yazılacak (aşşağıya da!)

        // fetchedToken =
        //     "007eJxTYLBtXJ60N/ftTZ5k3dJnrH1RFV881VayK7yLaGKNy6/7L6TAYGScnGRhbG5gYWGQapJqYWiZaphqmmaSamSSmpZoYGgU/m1VckMgI8N2ywUMjFAI4jMzJCYlMzAAANaGHfs="; // alternatif çözüm olarak statik yazılacak (aşşağıya da!)
        Debug.Log("Fetched Token: " + fetchedToken);
        Join (fetchedToken, channel, userId);
    }

    public void JoinWithFetchedToken(
        string url,
        string channel,
        string userId,
        int TimeToLive
    )
    {
        StartCoroutine(FetchTokenForJoin(url, channel, userId, TimeToLive));
    }

    public void Join(string _token, string _channelName, string userId)
    {
        // Enables the audio module.
        RtcEngine.EnableAudio();

        // Sets the user role ad broadcaster.
        RtcEngine.SetClientRole(CLIENT_ROLE_TYPE.CLIENT_ROLE_BROADCASTER);

        // Joins a channel.
        RtcEngine.JoinChannelWithUserAccount (_token, _channelName, userId);
        Debug.Log (_token);
    }

    public void Leave()
    {
        // Leaves the channel.
        RtcEngine.LeaveChannel();

        // Disable the audio modules.
        RtcEngine.DisableAudio();
    }

    public void FetchTokenButton(
        string url,
        string channel,
        string userId,
        int TimeToLive
    )
    {
        StartCoroutine(FetchToken(url, channel, userId, TimeToLive));
    }

    // Fetches the <Vg k="VSDK" /> token
    IEnumerator
    FetchToken(string url, string channel, string userId, int TimeToLive) /* , Action<string> callback = null */
    {
        int role = 1;
        UnityWebRequest request =
            UnityWebRequest
                .Get(string
                    .Format("{0}/rtc/{1}/{2}/uid/{3}/?expiry={4}",
                    url,
                    channel,
                    role,
                    userId,
                    TimeToLive));

        yield return request.SendWebRequest();
        if (request.isNetworkError || request.isHttpError)
        {
            Debug.Log(request.error);

            /* callback(null); */
            yield break;
        }
        AgoraToken tokenInfo =
            JsonUtility.FromJson<AgoraToken>(request.downloadHandler.text);

        /* callback(tokenInfo.rtcToken); */
        fetchedToken = tokenInfo.rtcToken;
        // fetchedToken =
        //     "007eJxTYLBtXJ60N/ftTZ5k3dJnrH1RFV881VayK7yLaGKNy6/7L6TAYGScnGRhbG5gYWGQapJqYWiZaphqmmaSamSSmpZoYGgU/m1VckMgI8N2ywUMjFAI4jMzJCYlMzAAANaGHfs=";
    }
}

public class AgoraToken
{
    public string rtcToken;
}
