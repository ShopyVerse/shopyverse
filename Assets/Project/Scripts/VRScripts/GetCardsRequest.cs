using Newtonsoft.Json;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;
public class GetCardsRequest : MonoBehaviour
{
    private string url = "https://tpay-test.turkcell.com.tr:443/tpay/provision/services/restful/getCardToken/getCards/";

    TextMeshProUGUI gcText;
    TextMeshProUGUI responseText;
    GameObject checkCardButton;
    GameObject buyButton;
    GameObject changePaymentButton;
    string chosenCard;
    int count = 0;

   string priceTag = "2551";
    int priceTagValue;

    private void Start()
    {
        //int newPrice = int.Parse(priceTag, out priceTagValue);
    }
    public void GetCards()
    {
        StartCoroutine(MakeCardRequest());
    }
    public void ChangeCardCliscked()
    {
        count = 1;
    }
    IEnumerator MakeCardRequest()
    {

        var bodyRequest = new GetCardRequest()
        {

            requestHeader = new RequestHeader()
            {
                applicationName = "PAYCELLTEST",
                applicationPwd = "PaycellTestPassword",
                clientIPAddress = "10.252.187.81",
                transactionDateTime = "20160309084056197",
                transactionId = "12345678901234567893"
            },

            msisdn = "5380521479"
        };

        //bodyRequest.requestHeader = new RequestHeader();

        var body = JsonConvert.SerializeObject(bodyRequest);

        Debug.Log(body);

        UnityWebRequest request = new UnityWebRequest(url, "POST");
        byte[] data = new System.Text.UTF8Encoding().GetBytes(body); // important
        request.uploadHandler = (UploadHandler)new UploadHandlerRaw(data);
        request.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");


        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.ConnectionError || request.result == UnityWebRequest.Result.ProtocolError)
        {
            Debug.Log(request.error);
        }
        else
        {


            string reString;
            reString = request.downloadHandler.text;
            Response response = JsonConvert.DeserializeObject<Response>(reString);
           

            Debug.Log(response.cardList.Count);
             

            //Debug.Log(response.cardList[0].cardId);
            //Debug.Log(response.cardList[0].cardBrand);
            //Debug.Log(response.cardList[1].alias);
            //Debug.Log(response.cardList[1].cardBrand);

            gcText = GameObject.Find("gc").GetComponent<TextMeshProUGUI>();
            responseText = GameObject.Find("ResponseText").GetComponent<TextMeshProUGUI>();

            
            if (count == 0)
            {
                gcText.text = response.cardList[0].maskedCardNo;
                chosenCard = response.cardList[0].cardId;

            }
            else if (count == 1)
            {
                if(response.cardList[1] != null)
                {
                    gcText.text = response.cardList[1].maskedCardNo;
                    chosenCard = response.cardList[1].cardId;
                }
               
            }
            else
            {
                Debug.Log("no more card");
            }


            //StartCoroutine(MakeProvisionRequest(response.cardList[0].cardId));

        }
        request.Dispose();

    }


    public void OnBuyButton(float price)
    {
        
        StartCoroutine(MakeProvisionRequest(chosenCard, price));
    }
    IEnumerator MakeProvisionRequest(string thisCardId, float price)
    {
        var provisionRequest = new ProvisionRequest()
        {
            requestHeader = new RequestHeader()
            {
                applicationName = "PAYCELLTEST",
                applicationPwd = "PaycellTestPassword",
                clientIPAddress = "10.252.187.81",
                transactionDateTime = "20160309084056197",
                transactionId = "12345678901234567893"
            },

            msisdn = "5380521479",
            cardId = thisCardId,
            merchantCode = "2003",
            referenceNumber = "12333374401234567892",
            amount = price.ToString(),
            paymentType = "SALE",
            acquirerBankCode = "111",
            
        };
        
        var body = JsonConvert.SerializeObject(provisionRequest);

        UnityWebRequest request = new UnityWebRequest(url, "POST");
        byte[] data = new System.Text.UTF8Encoding().GetBytes(body); // important
        request.uploadHandler = (UploadHandler)new UploadHandlerRaw(data);
        request.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");

        yield return request.SendWebRequest();


        if (request.result == UnityWebRequest.Result.ConnectionError || request.result == UnityWebRequest.Result.ProtocolError)
        {
            Debug.Log(request.error);
        }
        else
        {


            string reString;
            reString = request.downloadHandler.text;
            RootProvisionResponse responsePV = JsonConvert.DeserializeObject<RootProvisionResponse>(reString);



            Debug.Log("amount" + provisionRequest.amount);
            responseText.text = responsePV.responseHeader.responseDescription;
            
          

        }
        request.Dispose();

    }
    public void ButtonActivate()
    {
        checkCardButton = GameObject.Find("CheckCardButton");
        buyButton = GameObject.Find("BuyButton");
        changePaymentButton = GameObject.Find("ChangePaymentButton");
        checkCardButton.SetActive(false);
        buyButton.SetActive(true);
        changePaymentButton.SetActive(true);
    }
   
}
