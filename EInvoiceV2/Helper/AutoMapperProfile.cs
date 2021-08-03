using AutoMapper;
using EInvoiceCore.Entities;
using EInvoiceInfrastructure.Services.InvoiceHeaderServices.InvoiceVModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EInvoiceV2.Helper
{
    public class AutoMapperProfile : Profile
    {
        // mappings between model and entity objects
        public AutoMapperProfile()
        {
            CreateMap<InvoiceHeaderRequest, InvoiceHeader>();
        }
    }
}
