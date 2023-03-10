using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.XR.Interaction.Toolkit;

[RequireComponent(typeof(XRRayInteractor))]
public class CharacterClothing : MonoBehaviour
{
    [SerializeField] int distance;
    [SerializeField] GameObject hatContainer;
    [SerializeField] GameObject clothingUI;
    [SerializeField] TMP_Text clothesTxt;
    [SerializeField] TMP_Text priceTxt;
    [SerializeField] Button tryBtn;
    [SerializeField] Button buyBtn;
    List<GameObject> clothesCurrentlyTryOn = new List<GameObject>();
    List<GameObject> clothesCurrentlyWearing = new List<GameObject>();
    private XRRayInteractor rayInteractor = null;
    private void Awake()
    {
        rayInteractor = GetComponent<XRRayInteractor>();

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
            CheckClothingForActivateUI();
        }
    }

    void CheckClothingForActivateUI()
    {
        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out hit, distance))
        {
            Debug.DrawLine(ray.origin, hit.point, Color.red, 1f);
            
            ClickableItem item;
            if(hit.collider.TryGetComponent<ClickableItem>(out item)){
                
                var clickedTonHat = item.GetComponent<TonHat>();
                
                TonActivateClothingUI(clickedTonHat);

                tryBtn.onClick.AddListener(() => {
    
                foreach (var tonHat in FindObjectsOfType<TryHat>(true)) {
                 tonHat.gameObject.SetActive(false);
                 }
                var clickedTonHatID = clickedTonHat.id;
                foreach (var tonHat in FindObjectsOfType<TonHat>(true)) {
                var tonHatID = tonHat.id;
                if (tonHatID == clickedTonHatID && tonHat.GetComponent<ClickableItem>() == null) {
                tonHat.gameObject.SetActive(true);
                break; // Exit loop after activating the clicked TonHat object
                }
    }
});
              
                buyBtn.onClick.AddListener(()=>{
                    Api.Instance.RequestBuyWithTon();
                    buyBtn.onClick.RemoveAllListeners();
                });
                //Api.Instance.RequestBuyWithTon();
            }       
            Clothes clothes;
            if (hit.collider.TryGetComponent<Clothes>(out clothes))
            {                
                ActivateClothingUI(clothes);
            }
        }
    }
    public void TonActivateClothingUI(TonHat hat){
        clothingUI.SetActive(true);
        buyBtn.gameObject.SetActive(true);
        clothesTxt.SetText(hat.name);
        priceTxt.SetText(hat.price.ToString());
        tryBtn.onClick.RemoveAllListeners();
    }
    public void VRActivation()
    {
        if (rayInteractor != null)
        {

            if (rayInteractor.TryGetCurrent3DRaycastHit(out RaycastHit raycastHit))
            {
                Collider col = raycastHit.collider;
                if (col.TryGetComponent<Clothes>(out Clothes clothes))
                {
                    Debug.Log(clothes);
                    ActivateClothingUI(clothes);
                }
            }
        }

    }

    void ActivateClothingUI(Clothes clothes)
    {
        clothingUI.SetActive(true);
        buyBtn.gameObject.SetActive(true);
        clothesTxt.SetText(clothes.ObjectName);
        priceTxt.SetText(clothes.PriceTag.ToString());
        tryBtn.onClick.RemoveAllListeners();

        if (clothes is NFT)
        {
            NFT nft = (NFT)clothes;
            if (nft.isSold)
            {
                buyBtn.gameObject.SetActive(false);
                priceTxt.SetText("SOLD");
            }
        }
#if UNITY_WEBGL
        buyBtn.onClick.RemoveAllListeners();
        SetBuyButtonListeners(clothes);
#else 
        buyBtn.onClick.AddListener(() => GameObject.FindObjectOfType<GetCardsRequest>().OnBuyButton(clothes.PriceTag));
#endif
        SetTryButtonListeners(clothes);

        if (clothes is NFT)
            tryBtn.gameObject.SetActive(false);
    }
    void SetTryButtonListeners(Clothes clothes)
    {
        // TRY Button behaviors
        tryBtn.onClick.AddListener(() => clothes.Wear());

        if (clothesCurrentlyTryOn.Count <= 0)
            return;

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
            Api.Instance.RequestBuyWithTon();
            //Api.Instance.RequestBuyNFT(this, clothes);
        });
    }

    public void ApplyClothing(Clothes clothes)
    {
        Debug.Log("Wallet Transaction Completed");
        tryBtn.onClick?.Invoke();
        clothesCurrentlyWearing.Clear();

        if (clothes is NFT)
        {
            clothes.Sold();
            return;
        }

        clothesCurrentlyWearing.Add(clothes.clothingOjects[0]);
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
        if (ClothingInitializer.Instance == null)
            return;
        foreach (GameObject go in ClothingInitializer.Instance.Hats)
        {
            Clothes clothes;
            if (go.TryGetComponent<Clothes>(out clothes))
            {
                GameObject hat = Instantiate(go, hatContainer.transform);
                hat.transform.localPosition = Vector3.zero;
                hat.SetActive(false);
                Destroy(hat.GetComponent<Clothes>());
                clothes.clothingOjects.Add(hat);
            }
        }
    }
}
