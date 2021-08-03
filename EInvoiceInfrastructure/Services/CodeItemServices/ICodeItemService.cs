using EInvoiceCore.Entities;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace EInvoiceInfrastructure.Services.CodeItemServices
{
    public interface ICodeItemService
    {
        Task<IEnumerable<CodeItem>> GetAll();
        Task<IEnumerable<CodeItem>> AddDataFromExcelFile(IFormFile excelFile);
    }
}
