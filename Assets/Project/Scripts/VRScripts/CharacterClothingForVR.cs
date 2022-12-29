using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.XR.Interaction.Toolkit;


[RequireComponent(typeof(XRRayInteractor))]
public class CharacterClothingForVR : MonoBehaviour
{
    [SerializeField] int distance;

    [SerializeField] GameObject hatContainer;

    [SerializeField] GameObject clothingUI;
    [SerializeField] TMP_Text clothesTxt;
    [SerializeField] Button tryBtn;
    [SerializeField] Button buyBtn;

    List<GameObject> clothesCurrentlyTryOn;
    List<GameObject> clothesCurrentlyWearing;

    private XRRayInteractor rayInteractor = null;
    private Vector3 scaleChange;
    private void Awake()
    {
        rayInteractor = GetComponent<XRRayInteractor>();
       // scaleChange = new Vector3(2f, 2f, 2f);
    }
    private void Start()
    {
        clothesCurrentlyTryOn = new List<GameObject>();
        clothesCurrentlyWearing = new List<GameObject>();

        clothesCurrentlyWearing.Add(hatContainer.transform.GetChild(0).gameObject);
        foreach (GameObject clothes in clothesCurrentlyWearing)
        {
            clothes.SetActive(true);
        }

        InitializeHats();
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out hit, distance))
            {
                Debug.DrawLine(ray.origin, hit.point, Color.red);
                Clothes clothes;
                if (hit.collider.TryGetComponent<Clothes>(out clothes))
                {
                    ActivateClothingUI(clothes);
                }
            }
        }
    }
    public void VRActivation()
    {

        Clothes clothes;
        if(rayInteractor != null)
        {
           
            if (rayInteractor.TryGetCurrent3DRaycastHit(out RaycastHit raycastHit))
            {
               
                
                if (raycastHit.collider.TryGetComponent<Clothes>(out clothes))
                {
                    Debug.Log(clothes);
                    ActivateClothingUI(clothes);
                }
            }
        }
        
    }
    public void ActivateClothingUI(Clothes clothes)
    {
        clothingUI.SetActive(true);
        clothesTxt.SetText(clothes.name);

        tryBtn.onClick.RemoveAllListeners();
        buyBtn.onClick.RemoveAllListeners();

        SetTryButtonListeners(clothes);
        SetBuyButtonListeners(clothes);

    }

    void SetTryButtonListeners(Clothes clothes)
    {
        // TRY Button behaviors
        tryBtn.onClick.AddListener(() => clothes.Wear());

        // Player not wears the same clothing already
        if (!clothesCurrentlyTryOn.Contains(clothes.clothingOjects[0]))
        {
            tryBtn.onClick.AddListener(() =>
            {
                clothesCurrentlyTryOn.Clear();
                clothesCurrentlyTryOn.Add(clothes.clothingOjects[0]);
            });
        }
    }

    void SetBuyButtonListeners(Clothes clothes)
    {
        buyBtn.onClick.AddListener(() =>
        {
            tryBtn.onClick?.Invoke();

            // Wallet transactions here!
            Debug.Log("Wallet Transaction Completed");

            clothesCurrentlyWearing.Clear();
            clothesCurrentlyWearing.Add(clothes.clothingOjects[0]);
        });
    }

    private void OnTriggerExit(Collider other)
    {
        Debug.Log("OnTriggerExit");

        if (other.CompareTag("Store"))
        {
            TakeOffTryOnClothing(); 
            clothingUI.SetActive(false);
        }
    }

    void TakeOffTryOnClothing()
    {
        foreach (GameObject tryOnClothes in clothesCurrentlyTryOn)
        {
            tryOnClothes.SetActive(false);
        }

        foreach (GameObject wearingClothes in clothesCurrentlyWearing)
        {
            wearingClothes.SetActive(true);
        }
    }

    void InitializeHats()
    {
        foreach (GameObject go in ClothingInitializer.Instance.Hats)
        {
            Clothes clothes;
            if (go.TryGetComponent<Clothes>(out clothes))
            {
                GameObject hat = Instantiate(go, hatContainer.transform);
                //hat.transform.localScale += scaleChange;

                hat.transform.localPosition = Vector3.zero;
                hat.SetActive(false);
                Destroy(hat.GetComponent<Clothes>());

                clothes.clothingOjects.Add(hat);
            }
        }
    }
}
