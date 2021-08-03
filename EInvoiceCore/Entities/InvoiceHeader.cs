using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace EInvoiceCore.Entities
{
    public class InvoiceHeader
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        //[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int InternalId { get; set; }
        public string UserId { get; set; }
        public string CustomerName { get; set; }
        public DateTime InvoiceDate { get; set; }
        public decimal TaxValue { get; set; }
        public decimal TotalAmount { get; set; }
        public decimal NetTotal { get; set; }
        public ICollection<InvoiceLine> InvoiceLines { get; set; }
    }
}





