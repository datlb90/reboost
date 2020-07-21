using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Reboost.DataAccess;
using Reboost.Service.Services;
using Reboost.WebApi.Email;
using Reboost.WebApi.Identity;
using VueCliMiddleware;

namespace Reboost.WebApi
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        readonly string MyAllowSpecificOrigins = "_myAllowSpecificOrigins";

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddCors(options =>
            {
                options.AddPolicy(name: MyAllowSpecificOrigins,
                              builder =>
                              {
                                  builder.WithOrigins("http://localhost:8081")
                                                        .AllowAnyHeader()
                                                        .AllowAnyMethod();
                              });
            });

            // Configure Entityframecore with SQL SErver
            services.AddDbContext<ApplicationDbContext>(options =>
            {
                options.UseSqlServer(Configuration.GetConnectionString("ReboostDbContext"));
            });

            services.AddIdentity<IdentityUser, IdentityRole>(options =>
            {
                options.Password.RequireDigit = true;
                options.Password.RequireLowercase = true;
                options.Password.RequiredLength = 5;
            }).AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();

            services.AddAuthentication(auth =>
            {
                auth.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                auth.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidAudience = Configuration["AuthSettings:JwtAudience"],
                    ValidIssuer = Configuration["AuthSettings:JwtIssuer"],
                    RequireExpirationTime = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["AuthSettings:JwtSecret"])),
                    ValidateIssuerSigningKey = true
                };
            });

            services.AddDbContext<ReboostDbContext>(options =>
            {
                options.UseSqlServer(Configuration.GetConnectionString("ReboostDbContext"));
            });

            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddTransient<IDocumentService, DocumentService>();
            services.AddScoped<IAuthService, AuthService>();
            services.AddTransient<IMailService, SendGridMailService>();

            services.AddControllers();
            services.AddRazorPages();
            //services.AddSpaStaticFiles(configuration =>
            //{
            //    configuration.RootPath = "ClientApp/dist";
            //});
            services.AddMvc(option => option.EnableEndpointRouting = false);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            //app.UseHttpsRedirection();
            //app.UseRouting();
            app.UseCors(MyAllowSpecificOrigins);
            //app.UseSpaStaticFiles();
            //app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseDefaultFiles();
            app.UseStaticFiles();
            app.UseMvc(routes =>
            {
                routes.MapSpaFallbackRoute(
                    name: "spa-fallback",
                    defaults: new { controller = "CatchAll", action = "Index" });
            });

            //app.UseEndpoints(endpoints =>
            //{
            //    endpoints.MapControllers();
            //});

            //app.UseSpa(spa =>
            //{
            //    if (env.IsDevelopment())
            //        spa.Options.SourcePath = "ClientApp";
            //    else
            //        spa.Options.SourcePath = "dist";

            //    if (env.IsDevelopment())
            //    {
            //        spa.UseVueCli(npmScript: "serve");
            //    }

            //});
        }
    }
}
