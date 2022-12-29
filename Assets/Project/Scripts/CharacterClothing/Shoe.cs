using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shoe : Clothes
{
    public override void Wear(GameObject container = null)
    {
        base.Wear();
        
        foreach (Transform clothing in clothingOjects[0].transform.parent)
        {
            clothing.gameObject.SetActive(false);
        }

        foreach (Transform clothing in clothingOjects[1].transform.parent)
        {
            clothing.gameObject.SetActive(false);
        }

        clothingOjects[0].SetActive(true);
        clothingOjects[1].SetActive(true);
    }
}
