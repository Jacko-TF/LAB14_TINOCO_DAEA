namespace LAB14_TINOCO_DAEA.Models.Request
{
    public class RequestInvoiceV3
    {
        public DateTime Date { get; set; }
        public string InvoiceNumber { get; set; }
        public float Total { get; set; }
    }
}
