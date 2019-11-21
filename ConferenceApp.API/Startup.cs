using System.Text;
using AutoMapper;
using ConferenceApp.API.Interfaces;
using ConferenceApp.API.Mapping;
using ConferenceApp.API.Models;
using ConferenceApp.API.Repositories;
using ConferenceApp.API.Services.Account;
using ConferenceApp.API.Services.Authorization;
using ConferenceApp.API.Services.Jwt;
using ConferenceApp.API.Validators;
using ConferenceApp.Core.DataAccess;
using ConferenceApp.Core.DataModels;
using ConferenceApp.Core.Interfaces;
using ConferenceApp.Core.Repositories;
using ConferenceApp.Core.Services;
using ConferenceApp.Migrator;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;

namespace ConferenceApp.API
{
    public class Startup
    {
        public Startup( IConfiguration configuration )
        {
            Configuration = configuration;
        }


        public IConfiguration Configuration { get; }


        /// <summary>
        /// This method gets called by the runtime.
        /// Use this method to add services to the container.
        /// </summary>
        public void ConfigureServices( IServiceCollection services )
        {
            // Core.

            // Repositories.
            services.AddTransient<IReportRepository, ReportRepository>();
            services.AddTransient<IRequestRepository, RequestRepository>();
            services.AddTransient<ICollaboratorRepository, CollaboratorRepository>();

            // Services.
            services.AddTransient<IReportService, ReportService>();
            services.AddTransient<IRequestService, RequestService>();
            services.AddTransient<IDocumentService, DocumentService>();


            // API.
            
            // Authorization.
            services.AddSingleton<IJwtHandler, JwtHandler>();
            services.AddTransient<AuthorizationServiceMiddleware>();
            services.AddSingleton<IAccountService, AccountService>();
            services.AddSingleton<IAdminRepository, AdminRepository>();
            services.AddScoped<IAuthorizationService, AuthorizationService>();
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddSingleton<IPasswordHasher<Admin>, PasswordHasher<Admin>>();

            // Adapters.
            services.AddTransient<IReportRepositoryAdapter, ReportRepositoryAdapter>();
            
            // TODO. DI Trouble.
            services.AddTransient<IRequestRepositoryAdapter, RequestRepositoryAdapter>();

            services.AddDistributedMemoryCache();

            var jwtSection           = Configuration.GetSection( "jwt" );
            var jwtOptions           = new JwtOptions();
            jwtSection.Bind( jwtOptions );

            services
                .AddAuthentication()
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

            // Запуск мигратора.
            var connectionString = Configuration.GetConnectionString( "DefaultConnection" );
            DatabaseInitialization( connectionString );

            // Регистрация Linq2Db.
            LinqToDB.Data.DataConnection.DefaultSettings = new Linq2DbSettings( connectionString );
            services.AddSingleton<MainDb>();

            // Рагистрация автомаппера.
            var mapper = CreateAutoMapper();
            services.AddSingleton( mapper );

            services.AddCors();
            services
                .AddControllers()
                .AddNewtonsoftJson()
                .AddFluentValidation( fv => fv.RegisterValidatorsFromAssemblyContaining<ReportValidator>() );
        }


        /// <summary>
        /// This method gets called by the runtime.
        /// Use this method to configure the HTTP request pipeline.
        /// </summary>
        public void Configure( IApplicationBuilder app, IWebHostEnvironment env )
        {
            if( env.IsDevelopment() )
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseCors( builder => builder.AllowAnyOrigin() );

            app.UseHttpsRedirection();
            app.UseRouting();

            app.UseAuthorization();
            app.UseAuthentication();

            app.UseMiddleware<AuthorizationServiceMiddleware>();

            app.UseEndpoints( endpoints => { endpoints.MapControllers(); } );
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
            var mappingConfig = new MapperConfiguration( mc => { mc.AddProfile( new UserProfile() ); } );

            return mappingConfig.CreateMapper();
        }
    }
}