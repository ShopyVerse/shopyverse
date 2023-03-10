using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Api : MonoBehaviour
{
    List<KeyValuePair<CharacterClothing, Clothes>> InProgressBuy = new List<KeyValuePair<CharacterClothing, Clothes>>();
    List<StoreInventory> InProgressFetchItemsListed = new List<StoreInventory>();

    public static Api Instance;
    private void Awake()
    {
        Instance = this;
    }

    public void RequestBuyNFT(CharacterClothing characterClothing, Clothes clothes)
    {
        if (InProgressBuy.Count > 1)
            return;
        InProgressBuy.Add(new KeyValuePair<CharacterClothing, Clothes>(characterClothing, clothes));
        Bridge.BuyNFT(clothes.TokenId);
#if UNITY_EDITOR || UNITY_ANDROID
        ReceiveBuyNFT();
#endif

    }

    public void RequestBuyWithTon()
    {       
        Debug.Log("buy with ton calisti");
        Bridge.BuyWithTon();
    }

    public void RequestFetchItemsListed(StoreInventory storeInventory)
    {
        InProgressFetchItemsListed.Add(storeInventory);
        Bridge.RequestFetchItemsListed(storeInventory.StoreAddress);
#if UNITY_EDITOR || UNITY_ANDROID
        // For testing purpose
        
        Debug.Log("Request Fetch Item Listed - API");
        ReceiveFetchItemsListed(storeInventory.StoreAddress +":"+"11,12");
#endif
    }
    public void ReceiveBuyNFT()
    {
        InProgressBuy[0].Key.ApplyClothing(InProgressBuy[0].Value);
        InProgressBuy.Clear();
    }

    public void ReceiveFetchItemsListed(string retString)
    {
        string[] temp = retString.Split(":");
        string storeAddress = temp[0];
        string items = temp[1];

        foreach (StoreInventory si in InProgressFetchItemsListed)
        {
            if (si.StoreAddress.Equals(storeAddress))
            {
                si.EnableListedItems(items);
            }
        }
    }

    public List<NFTInventoryItem> RequestNFTList()
    {
        Debug.Log("Request NFT List");

        return new List<NFTInventoryItem>();
    }

    public void ReceiveNFTList()
    {
        Debug.Log("Receive NFT List");
    }
}
