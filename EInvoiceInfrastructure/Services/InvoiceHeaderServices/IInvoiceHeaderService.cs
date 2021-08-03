using EInvoiceCore.Entities;
using EInvoiceInfrastructure.Services.InvoiceHeaderServices.InvoiceVModels;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace EInvoiceInfrastructure.Services.InvoiceHeaderServices
{
    public interface IInvoiceHeaderService
    {
        Task<IEnumerable<InvoiceHeader>> GetAll();
        Task<IEnumerable<IdentityUser>> GetAllUsers();
        Task<IEnumerable<InvoiceHeader>> GetbyDate(DateTime from, DateTime to);
        Task<IEnumerable<InvoiceHeader>> GetbyName(string name);
        Task<IEnumerable<InvoiceHeader>> GetbyUser(string userId);
        Task Create(InvoiceHeaderRequest invoiceHeader);
        Task<InvoiceHeader> IvoiceDetails(int invoiceId);
        Task<InvoiceHeader> GetByNumber(int number);
    }
}
