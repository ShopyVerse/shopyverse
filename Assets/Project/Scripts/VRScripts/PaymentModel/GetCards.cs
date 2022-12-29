// Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);
public class RequestHeader
{
    public string applicationName { get; set; }
    public string applicationPwd { get; set; }
    public string clientIPAddress { get; set; }
    public string transactionDateTime { get; set; }
    public string transactionId { get; set; }
}

public class GetCardRequest //request olmalý
{
    public string msisdn { get; set; }
    
    public RequestHeader requestHeader { get; set; }
}

