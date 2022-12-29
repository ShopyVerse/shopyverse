using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class NFT : Clothes
{
    PhotonView PV;
    public bool isSold = false;

    public override void Sold()
    {
        PV = GetComponent<PhotonView>();
        PV.RPC("EnableSoldLabel", RpcTarget.All);
    }

    [PunRPC]
    private void EnableSoldLabel()
    {
        soldLabel.gameObject.SetActive(true);
        isSold = true;
    }
}
