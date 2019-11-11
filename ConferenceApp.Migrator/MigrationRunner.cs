using System.Reflection;
using Npgsql;

namespace ConferenceApp.Migrator
{
    public class MigrationRunner
    {
        private NpgsqlConnection DbConnection;
        private string           FactoryType = "postgres";
        private Assembly         Asm;

        public MigrationRunner( string cnnString )
        {
            Asm          = GetType().Assembly;
            DbConnection = new NpgsqlConnection( cnnString );
        }

        public void Run()
        {
            var migrator = new ThinkingHome.Migrator.Migrator(FactoryType, DbConnection, Asm);
            migrator.Migrate();
        }
    }
}