using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using OriginBank.Repository.Sql;
using Microsoft.EntityFrameworkCore;
using OriginBank.Repository;
using OriginBank.Web.Utilities;
using OriginBank.Web.Services;

namespace OriginBank.Web
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
            services.AddControllersWithViews();
            services.AddDbContext<OriginBankContext>(options =>
                    options.UseSqlServer(Configuration.GetConnectionString("OriginBankContext")));

            var db = new OriginBankContext(new DbContextOptionsBuilder<OriginBankContext>()
                .UseSqlServer(Configuration.GetConnectionString("OriginBankContext")).Options);

            services.AddScoped<ICardRepository, SqlCardRepository>(_ => new SqlCardRepository(db));
            services.AddScoped<IOperationRepository, SqlOperationRepository>(_ => new SqlOperationRepository(db));
            services.AddScoped<ATMService>();

            services.AddSession(options =>
            {
                options.IdleTimeout = TimeSpan.FromSeconds(30);
                options.Cookie.HttpOnly = true;
                options.Cookie.IsEssential = true;
            });

            services.AddSingleton<ITerminalSessionManager, BasicTerminalSessionManager>();
        }
    

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/ATM/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();
           
            app.UseSession();
            app.UseCookiePolicy();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
