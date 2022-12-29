using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClothingInitializer : MonoBehaviour
{
    static ClothingInitializer instance;
    public static ClothingInitializer Instance { get { return instance; } }

    [SerializeField] GameObject[] hats;
    public GameObject[] Hats { get { return hats; } }

    private void Awake()
    {
        if (instance != null && instance != this)
            Destroy(this.gameObject);
        else
            instance = this;
    }
}
