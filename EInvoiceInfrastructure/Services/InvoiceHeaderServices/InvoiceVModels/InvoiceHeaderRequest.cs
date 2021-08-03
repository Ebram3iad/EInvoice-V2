using EInvoiceCore.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace EInvoiceInfrastructure.Services.InvoiceHeaderServices.InvoiceVModels
{
    public class InvoiceHeaderRequest
    {
        [Required]
        public int InternalId { get; set; }
        public string UserId { get; set; }

        [Required]
        public string CustomerName { get; set; }
        [Required]
        public DateTime InvoiceDate { get; set; }
        [Required]
        public decimal TaxValue { get; set; }
        public decimal TotalAmount { get; set; }
        public decimal NetTotal { get; set; }
        public ICollection<InvoiceLine> InvoiceLines { get; set; }
        // This will set the default value to a new list instead of null
        //public List<InvoiceLine> InvoiceLines { get; set; } = new List<InvoiceLine>();

    }
}
