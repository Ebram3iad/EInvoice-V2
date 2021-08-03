using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EInvoiceCore.Entities;
using EInvoiceInfrastructure.Services.EmailServices;
using EInvoiceInfrastructure.Services.InvoiceHeaderServices;
using EInvoiceInfrastructure.Services.InvoiceHeaderServices.InvoiceVModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace EInvoice.Web.Controllers
{
    public class InvoiceController : Controller
    {
        #region Feilds
        private readonly IInvoiceHeaderService _invoiceHeaderService;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IEmailService _emailService;
        #endregion

        #region Constructor
        public InvoiceController(IInvoiceHeaderService invoiceHeaderService, IEmailService emailService, IHttpContextAccessor httpContextAccessor, UserManager<IdentityUser> userManager)
        {
             
            _invoiceHeaderService = invoiceHeaderService;
            _httpContextAccessor = httpContextAccessor;
            _userManager = userManager;
            _emailService = emailService;

        }
        #endregion

        #region Actions
     
        [Authorize]
        public async Task<IActionResult> Index(string SearchString,int SearchNumber,DateTime from,DateTime to,string userId)
        {
            var users = await _invoiceHeaderService.GetAllUsers();
            if (users != null && users.Count() != 0)
            {
                ViewBag.AllUsers = users;
            }
            if (!String.IsNullOrEmpty(SearchString))
            {
                return View(await _invoiceHeaderService.GetbyName(SearchString));
            }
            else if (!String.IsNullOrEmpty(userId))
            {
                return View(await _invoiceHeaderService.GetbyUser(userId));
            }
            else if (SearchNumber>0)
            {
                List<InvoiceHeader> invoiceHeaders = new List<InvoiceHeader>();
                invoiceHeaders.Add(await _invoiceHeaderService.GetByNumber(SearchNumber));
               return View(invoiceHeaders);
            }
            else if ((from != null && from != default(DateTime)) && (to != null && to != default(DateTime)))
            {
                var invoiceHeader = await _invoiceHeaderService.GetbyDate(from,to);
               return View(invoiceHeader);
            }
            else
            return View(await _invoiceHeaderService.GetAll());
        }
        
        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetByNumber(int number)
        {
            var invoiceHeaders = await _invoiceHeaderService.GetByNumber(number);
            return RedirectToAction("Index" ,invoiceHeaders);
        }

        [Authorize]
        public async Task<ActionResult> CreateInvoice()
        {
            var user = await GetCurrentUserAsync();
            var model = new InvoiceHeaderRequest()
            {
                CustomerName = "",
                InternalId = 0,
                UserId=user.Id,
                InvoiceDate = DateTime.Now,
                TaxValue = 0,
                TotalAmount = 0,
                NetTotal = 0,
                InvoiceLines = new List<InvoiceLine> { }

            };
            return View(model);
        }

        [Authorize]
        [HttpPost]
       //[ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateInvoice( InvoiceHeaderRequest model)
        {
            if (ModelState.IsValid && model.InvoiceLines != null&& model.InvoiceLines.Count!=0)
            {
                var user = await GetCurrentUserAsync();
                model.UserId = user.Id;
                await _invoiceHeaderService.Create(model);
                try
                {
                    SendEmail(user.Email);

                }
                catch (Exception)
                {

                    throw;
                }
                return RedirectToAction("Index");
            }
            return View(model);
        }
       

        public async Task<IActionResult> Details(int id)
        {
            if (id == 0)
            {
                return NotFound();
            }

            var invoice =await _invoiceHeaderService.IvoiceDetails(id);
            if (invoice == null)
            {
                return NotFound();
            }

            return View(invoice);
        }


        #endregion
        #region Helpers

        private Task<IdentityUser> GetCurrentUserAsync()
        {
            return _userManager.GetUserAsync(HttpContext.User);
        }
       
        //Send Mail to user
        private void SendEmail(string toUserMail)
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