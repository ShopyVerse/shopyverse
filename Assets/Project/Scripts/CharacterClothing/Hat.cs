using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hat : Clothes
{
    public override void Wear(GameObject container = null)
    {
        base.Wear();

        foreach (Transform clothing in clothingOjects[0].transform.parent)
        {
            clothing.gameObject.SetActive(false);
        }

        //Instantiate(clothingOjects[0], Vector3.zero, Quaternion.identity, container.transform);

        clothingOjects[0].SetActive(true);
    }
}
