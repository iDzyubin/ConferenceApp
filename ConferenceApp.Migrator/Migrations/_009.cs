using ThinkingHome.Migrator.Framework;

namespace ConferenceApp.Migrator.Migrations
{
    [Migration( 9 )]
    public class _009 : Migration
    {
        #region Script

        private string Script => @"
            alter table cf.users
            rename column organization to organisation;
        ";

        #endregion
        
        public override void Apply() => Database.ExecuteNonQuery( Script );
    }
}