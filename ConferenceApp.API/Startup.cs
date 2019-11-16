using System.Text;
using AutoMapper;
using ConferenceApp.API.Mapping;
using ConferenceApp.API.Models;
using ConferenceApp.API.Services.Account;
using ConferenceApp.API.Services.Authorization;
using ConferenceApp.API.Services.Jwt;
using ConferenceApp.Core.DataAccess;
using ConferenceApp.Core.DataModels;
using ConferenceApp.Core.Interfaces;
using ConferenceApp.Core.Repositories;
using ConferenceApp.Core.Services;
using ConferenceApp.Migrator;
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
            services.AddTransient<IReportService, ReportService>();
            services.AddTransient<IRequestService, RequestService>();
            
            services.AddSingleton<IDocumentService, DocumentService>();
            services.AddTransient<IReportRepository, ReportRepository>();
//            services.AddScoped<IAdminRepository, AdminRepository>();
            services.AddTransient<IRequestRepository, RequestRepository>();

//
//            services.AddSingleton<IAccountService,        AccountService>();
//            services.AddSingleton<IJwtHandler,            JwtHandler>();
//            services.AddSingleton<IPasswordHasher<Admin>, PasswordHasher<Admin>>();
//            services.AddScoped<IAuthorizationService,  AuthorizationService>();
//            services.AddSingleton<IHttpContextAccessor,   HttpContextAccessor>();
//            services.AddTransient<AuthorizationServiceMiddleware>();
//            services.AddDistributedMemoryCache();


//            var jwtSection = Configuration.GetSection( "jwt" );
//            var jwtOptions = new JwtOptions();
//            jwtSection.Bind( jwtOptions );
//
//            services.AddAuthentication()
//                .AddJwtBearer( cfg =>
//                    {
//                        cfg.TokenValidationParameters = new TokenValidationParameters
//                        {
//                            IssuerSigningKey =
//                                new SymmetricSecurityKey( Encoding.UTF8.GetBytes( jwtOptions.SecretKey ) ),
//                            ValidIssuer = jwtOptions.Issuer,
//                            ValidateAudience = false,
//                            ValidateLifetime = true
//                        };
//                    }
//                );
//            services.Configure<JwtOptions>( jwtSection );

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
            services.AddControllers().AddNewtonsoftJson();
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

//            app.UseMiddleware<AuthorizationServiceMiddleware>();

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