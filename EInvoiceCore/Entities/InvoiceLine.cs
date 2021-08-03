using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace EInvoiceCore.Entities
{
    public class InvoiceLine
    {
        public int Id { get; set; }
        public int InvoiceID { get; set; }
        [ForeignKey("InvoiceID")]
        public InvoiceHeader Invoice { get; set; }
        public string ProductName { get; set; }
        public string ProductNo { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
        public decimal Total { get; set; }
    }
}





