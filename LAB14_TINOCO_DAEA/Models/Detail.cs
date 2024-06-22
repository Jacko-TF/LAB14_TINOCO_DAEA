namespace LAB14_TINOCO_DAEA.Models
{
    public class Detail
    {
        public int DetailID { get; set; }
        public int Amount { get; set; }
        public float Price { get; set; }
        public float SubTotal { get; set; }

        /*Llave foranea Product*/
        public Product Product { get; set; }
        public int ProductID { get; set; }

        /*Llave foranea Invoice*/
        public Invoice Invoice { get; set; }
        public int InvoiceID { get; set; }
    }
}
