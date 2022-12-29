using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using Photon.Pun;

public class VRPlayerLineVisual : MonoBehaviour
{
    public LineRenderer lineRendererLeft;
    public LineRenderer lineRendererRight;
    public XRInteractorLineVisual lineVisualLeft;
    public XRInteractorLineVisual lineVisualRight;
    
    public CharacterClothing characterClothing;

    PhotonView pv;
    // Start is called before the first frame update
    void Start()
    {
        pv = GetComponent<PhotonView>();
        if (pv.IsMine)
        {
            
        }
        else
        {
            //lineRendererLeft.enabled = false;
            lineRendererRight.enabled = false;
            //lineVisualLeft.enabled = false;
            lineVisualRight.enabled = false;

            characterClothing.enabled = false;
        }
    }

}
