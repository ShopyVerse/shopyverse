using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class NFTInventoryItem : MonoBehaviour
{
    private string itemName;
    public string ItemName
    {
        get { return itemName; }
        set
        {
            itemName = value;
            name_TXT.SetText(itemName);
        }
    }

    private Sprite itemImage;
    public Sprite ItemImage
    {
        get { return itemImage; }
        set
        {
            itemImage = value;
            image.sprite = itemImage;
        }

    }


    [SerializeField] private TMP_Text name_TXT;
    [SerializeField] private Image image;
}
