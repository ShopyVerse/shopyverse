using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NFTInventoryController : MonoBehaviour
{
    [SerializeField] List<NFTInventoryItem> inventoryItemList;
    [SerializeField] Transform inventoryItemRoot;

    [SerializeField] GameObject inventoryItemPrefab;


    [SerializeField] Sprite denemeSprite;

    void GetNFTList()
    {
        inventoryItemList = Api.Instance.RequestNFTList();
    }

    public void UpdateInventory()
    {
        StartCoroutine(UpdateInventoryRoutine());
    }

    IEnumerator UpdateInventoryRoutine()
    {
        foreach (Transform child in inventoryItemRoot)
        {
            Destroy(child.gameObject);
        }

        for (int i = 0; i < 23; i++)
        {
            GameObject inventoryItemObject = Instantiate(inventoryItemPrefab, inventoryItemRoot);
            inventoryItemObject.TryGetComponent<NFTInventoryItem>(out NFTInventoryItem item);

            if (item == null)
                continue;

            item.ItemName = "Item : " + i;
            item.ItemImage = denemeSprite;

            inventoryItemList.Add(item);

            yield return new WaitForSeconds(.1f);
        }
    }
}
