namespace LAB14_TINOCO_DAEA.Models.Request
{
    public class RequestInvoiceV2
    {
        public int CustomerID { get; set; }
        public List<RequestInvoiceV3> requestInvoiceV3s { get; set; }
    }
}
