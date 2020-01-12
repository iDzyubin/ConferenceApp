using ThinkingHome.Migrator.Framework;

namespace ConferenceApp.Migrator.Migrations
{
    [Migration( 8 )]
    public class _008 : Migration
    {
        #region Script

        private string Script => @"
            drop table cf.reports_in_session;
            drop table cf.sessions;

            create table cf.sections
            (
                id    uuid              NOT NULL PRIMARY KEY,
                title character varying NOT NULL
            );
            
            create table cf.reports_in_section
            (
                section_id uuid not null,
                report_id  uuid not null,
                FOREIGN KEY (section_id) REFERENCES cf.sections (id),
                FOREIGN KEY (report_id)  REFERENCES cf.reports (id)
            );
        ";

        #endregion
        
        public override void Apply() => Database.ExecuteNonQuery( Script );
    }
}