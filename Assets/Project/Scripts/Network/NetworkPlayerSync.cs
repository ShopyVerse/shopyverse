using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using TMPro;
public class NetworkPlayerSync : MonoBehaviour
{
    [SerializeField]MonoBehaviour[] playerScripts;
    [SerializeField] Camera playerCamera;
    PhotonView pv;
     public TextMeshPro username;
    
    void Start()
    {
        pv = GetComponent<PhotonView>();
        if (pv.IsMine)
        {
            username.text = PlayerPrefs.GetString("Username");
        }
        else
        {
            username.text = pv.Owner.NickName;
            playerCamera.enabled = false;
            foreach (MonoBehaviour item in playerScripts)
            {
                item.enabled = false;
            }
        }
    }
}
