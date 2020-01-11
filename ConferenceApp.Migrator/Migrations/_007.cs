using ThinkingHome.Migrator.Framework;

namespace ConferenceApp.Migrator.Migrations
{
    [Migration( 7 )]
    public class _007 : Migration
    {
        #region Script

        private string Script => @"
            create table cf.sessions
            (
                id    uuid              NOT NULL PRIMARY KEY,
                title character varying NOT NULL
            );
            
            create table cf.reports_in_session
            (
                session_id uuid not null,
                report_id  uuid not null,
                FOREIGN KEY (session_id) REFERENCES cf.sessions (id),
                FOREIGN KEY (report_id)  REFERENCES cf.reports (id)
            );
        ";

        #endregion
        
        public override void Apply() => Database.ExecuteNonQuery( Script );
    }
}