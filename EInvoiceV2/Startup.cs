using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using EInvoiceInfrastructure;
using EInvoiceInfrastructure.EFRepository;
using EInvoiceCore.Entities;
using EInvoiceInfrastructure.Services.InvoiceHeaderServices;
using EInvoiceInfrastructure.Services.CodeItemServices;
using AutoMapper;
using EInvoiceV2.Helper;
using EInvoiceInfrastructure.Services;
using EInvoiceInfrastructure.Services.EmailServices;
using Microsoft.Extensions.Options;

namespace EInvoiceV2
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<DBContext>(options =>
                options.UseSqlServer(
                    Configuration.GetConnectionString("DefaultConnection")));
            services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
                .AddEntityFrameworkStores<DBContext>();
            services.AddControllersWithViews();
            services.AddRazorPages();
            services.AddMvc();
            services.AddControllersWithViews();

            //options.UseSqlServer(conn));
            var mapperConfig = new MapperConfiguration(mc =>
            {
                mc.AddProfile(new AutoMapperProfile());
            });
            IMapper mapper = mapperConfig.CreateMapper();

            services.Configure<AppSettings>(Configuration)
                .AddSingleton(sp => sp.GetRequiredService<IOptions<AppSettings>>().Value);
            services.Configure<AppSettings>(Configuration.GetSection("AppSettings"));

            #region Common
            services.AddSingleton(mapper);

            #endregion

            #region Repositories
            services.AddScoped<IRepository<InvoiceHeader>, Repository<InvoiceHeader>>();
            services.AddScoped<IRepository<InvoiceLine>, Repository<InvoiceLine>>();
            services.AddScoped<IRepository<CodeItem>, Repository<CodeItem>>();

            #endregion

            #region Services
            services.AddScoped<IInvoiceHeaderService, InvoiceHeaderService>();
            services.AddScoped<ICodeItemService, CodeItemService>();
            services.AddScoped<IEmailService, EmailService>();

            #endregion
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                //app.UseDatabaseErrorPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
                endpoints.MapRazorPages();
            });
        }
    }
}
