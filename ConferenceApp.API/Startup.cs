using ConferenceApp.Core.DataAccess;
using ConferenceApp.Core.DataModels;
using ConferenceApp.Migrator;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace ConferenceApp.API
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
            // Запуск мигратора.
            var connectionString = Configuration.GetConnectionString("DefaultConnection");
            DatabaseInitialization(connectionString);
            
            // Регистрация Linq2Db.
            LinqToDB.Data.DataConnection.DefaultSettings = new Linq2DbSettings(connectionString);
            services.AddSingleton<MainDb>();
            
            services.AddControllers();
        }

        
        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure( IApplicationBuilder app, IWebHostEnvironment env )
        {
            if( env.IsDevelopment() )
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

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
    }
}