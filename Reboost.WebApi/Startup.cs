using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using Reboost.DataAccess;
using Reboost.DataAccess.Entities;
using Reboost.Service.Services;
using Reboost.WebApi.Identity;
using Reboost.WebApi.Utils;
using Stripe;
using System.Text;
using Hangfire;
using Hangfire.MemoryStorage;
using Microsoft.AspNetCore.Http;

namespace Reboost.WebApi
{
    public class Startup
    {
        private readonly IWebHostEnvironment _currentEnvironment;
        public Startup(IConfiguration configuration, IWebHostEnvironment currentEnvironment)
        {
            Configuration = configuration;
            _currentEnvironment = currentEnvironment;
        }

        public IConfiguration Configuration { get; }

        readonly string MyAllowSpecificOrigins = "_myAllowSpecificOrigins";

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            if (_currentEnvironment.IsDevelopment())
            {
                services.AddCors(options =>
                {
                    options.AddPolicy(name: MyAllowSpecificOrigins,
                                  builder =>
                                  {
                                      builder.WithOrigins("http://localhost:3011")
                                                            .AllowAnyHeader()
                                                            .AllowAnyMethod();
                                  });

                    options.AddPolicy("AllowAll", builder => builder.AllowAnyOrigin()
                                                                .AllowAnyHeader()
                                                                .AllowAnyMethod());

                });
            }

            // Configure Entityframecore with SQL SErver
            services.AddDbContext<ApplicationDbContext>(options =>
            {
                options.UseSqlServer(Configuration.GetConnectionString("ReboostDbContext"),
                    options => options.EnableRetryOnFailure(
                     maxRetryCount: 5,
                     maxRetryDelay: System.TimeSpan.FromSeconds(60),
                     errorNumbersToAdd: null
                    ));
            });

            services.AddIdentity<ApplicationUser, IdentityRole>(options =>
            {
                options.Password.RequireDigit = false;
                options.Password.RequiredLength = 6;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = false;
                options.Password.RequireLowercase = false;
            })
                .AddEntityFrameworkStores<ApplicationDbContext>()
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
            services.AddAuthentication()
                .AddGoogle("google", opt =>
                {
                    var googleAuth = Configuration.GetSection("Authentication:Google");

                    opt.ClientId = googleAuth["ClientId"];
                    opt.ClientSecret = googleAuth["ClientSecret"];
                    opt.SignInScheme = IdentityConstants.ExternalScheme;
                });
            services.AddAuthentication()
                .AddFacebook("facebook", opt => {
                    var facebookAuth = Configuration.GetSection("Authentication:Facebook");
                    opt.AppId = facebookAuth["AppId"];
                    opt.AppSecret = facebookAuth["AppSecret"];
                    opt.SignInScheme = IdentityConstants.ExternalScheme;
                });


            services.AddDbContext<ReboostDbContext>(options =>
            {
                options.UseSqlServer(Configuration.GetConnectionString("ReboostDbContext"), b => b.MigrationsAssembly("Reboost.DataAccess"));
            });

            services.AddHangfire(config =>
            {
                config.UseMemoryStorage();
            });

            services.AddAutoMapper(typeof(AutoMapperProfile).Assembly);


            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddTransient<IDocumentService, DocumentService>();
            services.AddTransient<IRequestQueueService, RequestQueueService>();
            services.AddScoped<IRaterService, RaterService>();
            services.AddScoped<ILookUpService, LookUpService>();
            services.AddScoped<IQuestionsService, QuestionsService>();
            services.AddScoped<IQuestionPartService, QuestionPartService>();
            services.AddScoped<ISampleService, SampleService>();
            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IDataIngestionService, DataIngestionService>();
            services.AddTransient<Reboost.Service.Services.IMailService, Reboost.Service.Services.SendGridMailService>();
            services.AddScoped<IPDFService, PDFService>();
            services.AddScoped<IRubricService, RubricService>();
            services.AddScoped<ISubmissionService, SubmissionService>();
            services.AddScoped<IDiscussionService, DiscussionService>();
            services.AddScoped<IPaymentService, PaymentService>();
            services.AddScoped<IStripeService, StripeService>();
            services.AddScoped<IReviewService, Reboost.Service.Services.ReviewService>();
            services.AddScoped<IArticlesService, ArticlesService>();
            services.AddScoped<IChatGPTService, ChatGPTService>();
            services.AddScoped<IOrderService, Reboost.Service.Services.OrderService>();

            services.AddControllers().AddNewtonsoftJson(option => option.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore);
            services.AddRazorPages();
            //services.AddSpaStaticFiles(configuration =>
            //{
            //    configuration.RootPath = "ClientApp/dist";
            //});
            services.AddMvc(option => option.EnableEndpointRouting = false);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IReviewService reviewService)
        {
            StripeConfiguration.ApiKey = "sk_test_51I9tu1D04tWYlOu2YZ56fZ4sMfOsXSmRdJ0t3iu5fzdeyOVtL6w3rSb74NTh45kNeDKcbrH9uTQnigaoCwixS9y100zPTgMYoc";

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            // Add Hangfire
            app.UseHangfireDashboard();
            app.UseHangfireServer();

            app.UseGlobalExceptionHandler();

            //app.UseHttpsRedirection();
            //app.UseRouting();
            app.UseCors(MyAllowSpecificOrigins);
            //app.UseSpaStaticFiles();
            //app.UseRouting();

            app.UseCookiePolicy(new CookiePolicyOptions
            {
                MinimumSameSitePolicy = SameSiteMode.Lax,
                Secure = CookieSecurePolicy.Always
            });

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

            System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);

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
