using ThinkingHome.Migrator.Framework;

namespace ConferenceApp.Migrator.Migrations
{
    [Migration( 10 )]
    public class _010 : Migration
    {
        #region Script

        private string Script => @"
            create table cf.compilations
            (
                id   uuid              NOT NULL PRIMARY KEY,
                path character varying NOT NULL
            );
        ";

        #endregion
        
        public override void Apply() => Database.ExecuteNonQuery( Script );
    }
}