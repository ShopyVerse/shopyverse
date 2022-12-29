//// Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);
//public class RequestHeader
//{
//    public string applicationName { get; set; }
//    public string applicationPwd { get; set; }
//    public string clientIPAddress { get; set; }
//    public string transactionDateTime { get; set; }
//    public string transactionId { get; set; }
//}

public class ProvisionRequest
{
    public RequestHeader requestHeader { get; set; }
    public string cardId { get; set; }
    public string merchantCode { get; set; }
    public string msisdn { get; set; }
    public string referenceNumber { get; set; }
    public string amount { get; set; }
    public string paymentType { get; set; }
    public string acquirerBankCode { get; set; }

}