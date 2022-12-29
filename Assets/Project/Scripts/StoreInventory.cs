using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoreInventory : MonoBehaviour
{
    [SerializeField] string storeAddress;
    public string StoreAddress { get { return storeAddress; } }
    [SerializeField] Clothes[] listedClothes;

    void Start()
    {
        CheckListedNFTs();
    }

    void CheckListedNFTs()
    {
        Api.Instance.RequestFetchItemsListed(this);
    }

    public void EnableListedItems(string listedItems)
    {
        string[] items = listedItems.Split(",");
        List<string> itemsList = new List<string>(items);

        //If Dynamic Token Ids for nfts
        /* for (int i = 0; i < listedClothes.Length; i++)
        {
            Clothes currentClothing = listedClothes[i];

            if (i < items.Length) currentClothing.TokenId = items[i];

            if (currentClothing.TokenId.Equals("-1"))
            {
                currentClothing.Sold();
            }
        } */

        //If Static Token Ids for nfts
        foreach (Clothes clt in listedClothes)
        {
            if (!itemsList.Contains(clt.TokenId))
            {
                clt.Sold();
            }
        }

    }
}
