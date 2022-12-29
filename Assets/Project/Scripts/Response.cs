// Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);
using System.Collections.Generic;

public class CardList
{
    public string cardId { get; set; }
    public string maskedCardNo { get; set; }
    public string alias { get; set; }
    public string cardBrand { get; set; }
    public bool isDefault { get; set; }
    public bool isExpired { get; set; }
    public bool showEulaId { get; set; }
    public bool isThreeDValidated { get; set; }
    public bool isOTPValidated { get; set; }
    public string activationDate { get; set; }
    public object cardType { get; set; }
}

public class ResponseHeader
{
    public string transactionId { get; set; }
    public string responseDateTime { get; set; }
    public string responseCode { get; set; }
    public string responseDescription { get; set; }
}

public class Response
{
    public ResponseHeader responseHeader { get; set; }
    public object extraParameters { get; set; }
    public string eulaId { get; set; }
    public List<CardList> cardList { get; set; }
}
