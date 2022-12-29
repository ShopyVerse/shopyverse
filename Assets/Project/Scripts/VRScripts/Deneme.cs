using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit;

[RequireComponent(typeof(XRRayInteractor))]
public class Deneme : MonoBehaviour
{
    //[SerializeField] private InputActionReference activateReference = null;

    private XRRayInteractor rayInteractor = null;
    //private bool isEnabled = false;

    private void Awake()
    {
        rayInteractor = GetComponent<XRRayInteractor>();
        //rayInteractor.TryGetCurrentRaycast(out var hitPosition, out var hitNormal, out _, out _);
        
    }
    public void OnClicked()
    {
        if(rayInteractor.TryGetCurrent3DRaycastHit(out RaycastHit raycastHit))
        {
            Debug.Log(raycastHit.collider.tag);
        }
        Debug.Log("This button has clicked");
    }
}
