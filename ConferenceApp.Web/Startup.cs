using System.Text;
using AutoMapper;
using ConferenceApp.Core.DataAccess;
using ConferenceApp.Core.DataModels;
using ConferenceApp.Core.Interfaces;
using ConferenceApp.Core.Models;
using ConferenceApp.Core.Repositories;
using ConferenceApp.Core.Services;
using ConferenceApp.Migrator;
using ConferenceApp.Web.Mapping;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.SpaServices.ReactDevelopmentServer;
using ConferenceApp.Web.Models;
using ConferenceApp.Web.Services.Account;
using ConferenceApp.Web.Services.Authorization;
using ConferenceApp.Web.Services.Jwt;
using ConferenceApp.Web.ViewModels;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;

namespace ConferenceApp.Web
{
    public class Startup
    {
        public Startup( IConfiguration configuration )
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices( IServiceCollection services )
        {
            // Core.

            // Repositories.
            services.AddTransient<IUserRepository, UserRepository>();
            services.AddTransient<IReportRepository, ReportRepository>();

            // Services.
            services.AddTransient<IDocumentService, DocumentService>();
            services.AddTransient<IUserService, UserService>();
            
            
            // API.
            
            // Authorization.
            services.AddSingleton<IJwtHandler, JwtHandler>();
            services.AddTransient<AuthorizationServiceMiddleware>();
            services.AddSingleton<IAccountService, AccountService>();
            services.AddScoped<IAuthorizationService, AuthorizationService>();
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddSingleton<IPasswordHasher<SignInViewModel>, PasswordHasher<SignInViewModel>>();

            services.AddDistributedMemoryCache();

            var jwtSection           = Configuration.GetSection( "jwt" );
            var jwtOptions           = new JwtOptions();
            jwtSection.Bind( jwtOptions );

            services
                .AddAuthorization()
                .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer( cfg =>
                {
                    cfg.TokenValidationParameters = new TokenValidationParameters
                    {
                        IssuerSigningKey = new SymmetricSecurityKey
                        ( 
                            Encoding.UTF8.GetBytes( jwtOptions.SecretKey ) 
                        ),
                        ValidIssuer      = jwtOptions.Issuer,
                        ValidateAudience = false,
                        ValidateLifetime = true
                    };
                });
            services.Configure<JwtOptions>( jwtSection );

            services
                .AddControllersWithViews()
                // Регистрация валидаторов.
                .AddFluentValidation( configuration => configuration.RegisterValidatorsFromAssemblyContaining<Startup>() );
            services.AddRazorPages();

            // Запуск мигратора.
            var connectionString = Configuration.GetConnectionString( "ConferenceDbConnection" );
            DatabaseInitialization( connectionString );

            // Регистрация Linq2Db.
            LinqToDB.Data.DataConnection.DefaultSettings = new Linq2DbSettings( connectionString );
            services.AddSingleton<MainDb>();

            // Рагистрация автомаппера.
            var mapper = CreateAutoMapper();
            services.AddSingleton( mapper );
            
            // In production, the React files will be served from this directory
            services.AddSpaStaticFiles( configuration => { configuration.RootPath = "ClientApp/build"; } );
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure( IApplicationBuilder app, IWebHostEnvironment env )
        {
            if( env.IsDevelopment() )
            {
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
            }
            else
            {
                app.UseExceptionHandler( "/Error" );
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseSpaStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();
            app.UseEndpoints( endpoints =>
                {
                    endpoints.MapControllerRoute(
                        name: "default",
                        pattern: "{controller}/{action=Index}/{id?}"
                    );
                    endpoints.MapRazorPages();
                }
            );

            app.UseSpa( spa =>
                {
                    spa.Options.SourcePath = "ClientApp";

                    if( env.IsDevelopment() )
                    {
                        spa.UseReactDevelopmentServer( npmScript: "start" );
                    }
                }
            );
        }
        
        
        /// <summary>
        /// Инициализация БД.
        /// </summary>
        /// <param name="connectionString"></param>
        private void DatabaseInitialization( string connectionString )
        {
            var migrator = new MigrationRunner( connectionString );
            migrator.Run();
        }


        /// <summary>
        /// Регистрация автомаппера.
        /// </summary>
        /// <returns></returns>
        private IMapper CreateAutoMapper()
        {
            var mappingConfig = new MapperConfiguration( mc =>
            {
                mc.AddProfile<UserProfile>();
                mc.AddProfile<UserShortInfoProfile>();
                mc.AddProfile<SignUpProfile>();
                mc.AddProfile<ReportProfile>();
                mc.AddProfile<AttachReportProfile>();
                mc.AddProfile<Core.Mapping.ReportProfile>();
            } );
            return mappingConfig.CreateMapper();
        }
    }
}