using ThinkingHome.Migrator.Framework;

namespace ConferenceApp.Migrator.Migrations
{
    [Migration(5)]
    public class _005 : Migration
    {
        #region Script

        private string Script => @"
            alter table cf.users
            drop column fax,

            drop column address,
            add column organisation_address character varying,

            add column city character varying,
            
            drop column degree,
            
            add column position character varying;
        ";

        #endregion

        public override void Apply() => Database.ExecuteNonQuery( Script );
    }
}