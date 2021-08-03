using EInvoiceCore.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;

namespace EInvoiceInfrastructure
{
    public class DBContext : IdentityDbContext<IdentityUser>
    {
        public DBContext(DbContextOptions<DBContext> options)
            : base(options)
        {
        }
        public DbSet<InvoiceHeader> InvoiceHeaders { get; set; }
        public DbSet<InvoiceLine> InvoiceLines { get; set; }
        public DbSet<CodeItem> CodeItems { get; set; }
        
    }
}
