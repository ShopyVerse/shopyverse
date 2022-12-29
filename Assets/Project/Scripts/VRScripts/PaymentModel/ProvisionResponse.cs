// Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);
public class ResponseHeaderProvision
{
    public string transactionId { get; set; }
    public string responseDateTime { get; set; }
    public string responseCode { get; set; }
    public string responseDescription { get; set; }
}

public class RootProvisionResponse
{
    public ResponseHeader responseHeader { get; set; }
    public object extraParameters { get; set; }
    public string orderId { get; set; }
    public string acquirerBankCode { get; set; }
    public string issuerBankCode { get; set; }
    public string approvalCode { get; set; }
    public string reconciliationDate { get; set; }
}

