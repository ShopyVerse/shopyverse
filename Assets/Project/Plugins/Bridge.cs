using UnityEngine;
using System.Runtime.InteropServices;
using System.Collections;
using TMPro;

public static class BridgeInternal
{
    [DllImport("__Internal")]
    public static extern void BuyNFT(string tokenId);

    [DllImport("__Internal")]
    public static extern void RequestFetchItemsListed(string storeAddress);
}

public static class Bridge
{
    public static void BuyNFT(string tokenId)
    {
#if UNITY_WEBGL && !UNITY_EDITOR
        BridgeInternal.BuyNFT(tokenId);
#endif
    }

    public static void RequestFetchItemsListed(string storeAddress)
    {
#if UNITY_WEBGL && !UNITY_EDITOR
        BridgeInternal.RequestFetchItemsListed(storeAddress);
#endif
    }
}