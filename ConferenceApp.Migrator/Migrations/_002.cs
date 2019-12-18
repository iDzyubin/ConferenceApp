using ThinkingHome.Migrator.Framework;

namespace ConferenceApp.Migrator.Migrations
{
    [Migration(2)]
    public class _002 : Migration
    {
        #region Script

        private string Script => @"
            drop table cf.collaborators;
            
            alter table cf.reports
            add column collaborators character varying;
            
            alter table cf.users
            add column start_residence_date timestamp without time zone,
            add column end_residence_date timestamp without time zone
        ";

        #endregion

        public override void Apply() => Database.ExecuteNonQuery( Script );
    }
}