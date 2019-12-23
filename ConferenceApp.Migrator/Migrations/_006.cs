using ThinkingHome.Migrator.Framework;

namespace ConferenceApp.Migrator.Migrations
{
    [Migration( 6 )]
    public class _006 : Migration
    {
        #region Script

        private string Script => @"
            alter table cf.users
            add column confirm_code character varying;
        ";

        #endregion


        public override void Apply() => Database.ExecuteNonQuery( Script );
    }
}