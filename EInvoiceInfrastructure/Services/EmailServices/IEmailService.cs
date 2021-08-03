using System;
using System.Collections.Generic;
using System.Text;

namespace EInvoiceInfrastructure.Services.EmailServices
{
    public interface IEmailService
    {
            void Send(string to, string subject, string html, string from = null);
    }
}
