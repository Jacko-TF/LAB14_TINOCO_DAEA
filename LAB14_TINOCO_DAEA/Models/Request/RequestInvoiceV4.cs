namespace LAB14_TINOCO_DAEA.Models.Request
{
    public class RequestInvoiceV4
    {
        public int InvoiceID { get; set; }
        public List<RequestDetailV1> DetailV1s { get; set;}
    }
}
