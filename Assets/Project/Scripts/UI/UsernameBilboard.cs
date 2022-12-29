using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UsernameBilboard : MonoBehaviour
{
    void LateUpdate()
    {
        if (Camera.main is null) return;
        transform.LookAt(transform.position + Camera.main.transform.forward);
    }
}
