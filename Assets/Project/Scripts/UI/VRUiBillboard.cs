using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VRUiBillboard : MonoBehaviour
{
    [SerializeField] Vector3 uiPosition;
    void LateUpdate()
    {
        if (Camera.main is null) return;
        //transform.LookAt(uiPosition + Camera.main.transform.b);
    }
}
