using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using TMPro;
public class NetworkPlayerSync : MonoBehaviour
{
    [SerializeField]MonoBehaviour[] playerScripts;
    [SerializeField] Camera playerCamera;
    public GameObject a_chatObj;
    public PhotonView pv;
    public TextMeshPro username;
    
    private void Awake() {
        a_chatObj = GameObject.FindWithTag("AdminInput");
    }

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
        
    if (pv.IsMine)
        {
            if(pv.Owner.NickName != "BK")
            {
                a_chatObj.SetActive(false);
            }
        }
       
    }
}
