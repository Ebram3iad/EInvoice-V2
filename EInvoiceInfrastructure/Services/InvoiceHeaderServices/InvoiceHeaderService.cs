using AutoMapper;
using EInvoiceCore.Entities;
using EInvoiceInfrastructure.EFRepository;
using EInvoiceInfrastructure.Services.EmailServices;
using EInvoiceInfrastructure.Services.InvoiceHeaderServices.InvoiceVModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

namespace EInvoiceInfrastructure.Services.InvoiceHeaderServices
{
    public class InvoiceHeaderService : IInvoiceHeaderService
    {
        #region Fields

        private readonly DBContext _context;
        private readonly IRepository<InvoiceHeader> _invoiceHeaderRepository;
        private readonly IMapper _mapper;
        private readonly IEmailService _emailService;

        #endregion

        #region Constructor
        public InvoiceHeaderService(IMapper mapper, IEmailService emailService, DBContext context, IRepository<InvoiceHeader> invoiceHeaderRepository)
        {
            _invoiceHeaderRepository = invoiceHeaderRepository;
            _context = context;
            _emailService = emailService;
            _mapper = mapper;
        }
        #endregion

        #region Methods

        public async Task Create(InvoiceHeaderRequest model)
        {
            try
            {
                //SendEmail();
                //SendM();
                model.NetTotal = await CalulateNetValue(model);
                var invoiceHeader = _mapper.Map<InvoiceHeader>(model);
                await _invoiceHeaderRepository.Create(invoiceHeader);
                await _invoiceHeaderRepository.Save();
            }
            catch (Exception)
            {

                throw;
            }

        }

        //GetAllUsers
        public async Task<IEnumerable<IdentityUser>> GetAllUsers()
        {
            try
            {
                var invoiceHeaders = _context.InvoiceHeaders.Where(u=> u.UserId!=null).AsNoTracking().ToList();
                List<IdentityUser> users = new List<IdentityUser>();
                if (invoiceHeaders.Count != 0 && invoiceHeaders != null)
                {
                    foreach (var item in invoiceHeaders)
                    {
                        users.Add(_context.Users.Where(u => u.Id == item.UserId && item.UserId!=null).AsNoTracking().FirstOrDefault());
                    }
                }
                return users;
            }
            catch (Exception)
            {

                throw;
            }
        }
        public async Task<IEnumerable<InvoiceHeader>> GetAll()
        {
            try
            {
                var invoices = _context.InvoiceHeaders.Include(x => x.InvoiceLines).AsNoTracking().ToList();
                return invoices;
            }
            catch (Exception)
            {

                throw;
            }
        }
        public async Task<IEnumerable<InvoiceHeader>> GetbyDate(DateTime from, DateTime to)
        {
            try
            {
                var invoices = _context.InvoiceHeaders.Include(x => x.InvoiceLines).Where(x=> x.InvoiceDate>=from && x.InvoiceDate<=to ).AsNoTracking().ToList();
                return invoices;
            }
            catch (Exception)
            {

                throw;
            }
        }
        
        public async Task<IEnumerable<InvoiceHeader>> GetbyName(string name)
        {
            try
            {
                var invoices = _context.InvoiceHeaders.Include(x => x.InvoiceLines).Where(x=> x.CustomerName.Contains(name) ).AsNoTracking().ToList();
                return invoices;
            }
            catch (Exception)
            {

                throw;
            }
        }
        public async Task<IEnumerable<InvoiceHeader>> GetbyUser(string userId)
        {
            try
            {
                var invoices = _context.InvoiceHeaders.Include(x => x.InvoiceLines).Where(x=> x.UserId==userId ).AsNoTracking().ToList();
                return invoices;
            }
            catch (Exception)
            {

                throw;
            }
        }
        public async Task<InvoiceHeader> GetByNumber(int number)
        {
            try
            {
                var invoice = _context.InvoiceHeaders.Include(x => x.InvoiceLines).Where(x=> x.InternalId==number).AsNoTracking().FirstOrDefault();
                return invoice;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<InvoiceHeader> IvoiceDetails(int invoiceId)
        {
            var invoice = _context.InvoiceHeaders.Include(x => x.InvoiceLines.Where(i=> i.InvoiceID==invoiceId)).Where(i=> i.Id==invoiceId).AsNoTracking().FirstOrDefault();
            return invoice;
        }

        private async Task<decimal> CalulateNetValue(InvoiceHeaderRequest model)
        {
            if (await CalulateProductTotal(model)!=0)
            model.TotalAmount = await CalulateProductTotal(model);
            decimal taxValue = model.TotalAmount * (model.TaxValue/100);
            return (model.TotalAmount - taxValue);
        }

        private async Task<decimal> CalulateProductTotal(InvoiceHeaderRequest model)
        {
            decimal totalAmount = 0;
            if (model.InvoiceLines != null && model.InvoiceLines.Count > 0)
                foreach (var item in model.InvoiceLines)
                {
                    item.Total = item.Quantity * item.Price;
                    totalAmount += item.Total;
                }
            return (totalAmount);
        }
        //public void SendM()
        //{
        //    try
        //    {
        //        //// create message
        //        //var email = new MimeMessage();
        //        //email.From.Add(new MailboxAddress("Ivioce Created", "bobramg@gmail.com"));
        //        //email.To.Add(new MailboxAddress("Ebram", "ebram.f.ayad@gmail.com"));
        //        //email.Subject = "Test Mail";
        //        //email.Body = new TextPart("plain")
        //        //{
        //        //    Text = "Test Invoice Mail"
        //        //};

        //        //// send email
        //        //using var smtp = new SmtpClient();
        //        //smtp.Connect("smtp.gmail.com", 587, SecureSocketOptions.StartTls);
        //        //smtp.Authenticate("bobramg@gmail.com", "987654321@bob");
        //        //smtp.Send(email);
        //        //smtp.Disconnect(true);

        //        //MailMessage mail = new MailMessage();
        //        //mail.To.Add( "ebram.f.ayad@gmail.com");
        //        //mail.From = new MailAddress("kyler84@ethereal.email");
        //        //mail.Subject = "Test Mail";
        //        //mail.Body = "Test Invoice Mail";
        //        //mail.IsBodyHtml = true;
        //        //SmtpClient smtp = new SmtpClient("smtp.ethereal.email", 587);
        //        //smtp.EnableSsl = true;
        //        //smtp.UseDefaultCredentials = false;
        //        //smtp.Credentials = new System.Net.NetworkCredential("kyler84@ethereal.email", "xfRGPZt4XRkEjrBXfs");
        //        //smtp.Send(mail);




        //        SmtpClient client = new SmtpClient("smtp.gmail.com", int.Parse("587"));
        //        client.EnableSsl = true;
        //        client.Credentials = new NetworkCredential("doctor4me123@gmail.com", "Abc_123456"); //sender

        //        MailMessage mailMessage = new MailMessage();
        //        mailMessage.IsBodyHtml = true;
        //        mailMessage.From = new MailAddress("doctor4me123@gmail.com"); //sender
        //        mailMessage.Subject = "Test Mail";
        //        mailMessage.Body = "Test Invoice Mail Hello World";
        //        mailMessage.To.Add("doctor4me123@gmail.com");
        //        client.Send(mailMessage);

        //    }
        //    catch (Exception e)
        //    {
        //        string message = e.Message;
        //        throw;
        //    }
           
        //}
        private  void SendEmail(string toUserMail)
        {
            string message;

            message = $@"<p>Please use the below token to reset your password with the <code>/accounts/reset-password</code> api route:</p>";
                             
            _emailService.Send(
               to: toUserMail,
               subject: "Test Invoice",
               html: $@"<h4>Create Invoice Email</h4>
                         {message}"
           );
        }

        #endregion
    }
}
