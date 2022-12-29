using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Clothes : MonoBehaviour
{
    [SerializeField] private string objectName;
    [SerializeField] private string tokenId = "-1";
    [SerializeField] private float priceTag;

    [SerializeField] protected GameObject soldLabel;

    public List<GameObject> clothingOjects;

    public string ObjectName { get => objectName; set => objectName = value; }
    public string TokenId { get => tokenId; set => tokenId = value; }
    public float PriceTag { get => priceTag; set => priceTag = value; }

    public virtual void Wear(GameObject container = null)
    {
        IsClothingObjectsNull();
    }

    protected void IsClothingObjectsNull()
    {
        if (clothingOjects.Count <= 0)
        {
            Debug.Log("clothingOjects array is Empty");
            return;
        }
    }

    public virtual void Sold()
    {
        Debug.Log("Clothes Sold");

    }
}
