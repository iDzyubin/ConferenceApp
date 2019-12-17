using ThinkingHome.Migrator.Framework;

namespace ConferenceApp.Migrator.Migrations
{
    [Migration(3)]
    public class _003 : Migration
    {
        #region Script

        private string Script => @"
            drop table cf.requests;
            
            drop table cf.admins;
        ";

        #endregion
        
        public override void Apply() => Database.ExecuteNonQuery( Script );
    }
}