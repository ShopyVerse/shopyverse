using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit;

public class RaycastController : MonoBehaviour
{
    [SerializeField] InputActionReference raycastInputAction;

    public XRInteractorLineVisual lineVisual;
    // Start is called before the first frame update
  

    private void OnEnable()
    {
        raycastInputAction.action.performed += RaycastPressed;
    }

    private void RaycastPressed(InputAction.CallbackContext obj)
    {
        if (lineVisual.enabled == false)
        {
            lineVisual.enabled = true;
        }
        else
            lineVisual.enabled = false;
    }
   

}
