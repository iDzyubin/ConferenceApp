using ThinkingHome.Migrator.Framework;

namespace ConferenceApp.Migrator.Migrations
{
    [Migration(3)]
    public class _003 : Migration
    {
        #region Script

        private string Script => @"
            drop table cf.admins;

            alter table cf.reports
            drop column request_id,
            drop column collaborators; 

            drop table cf.requests;

            alter table cf.users
            add column password character varying not null,
            add column user_role integer not null default 0,
            add column user_status integer not null default 0;
            
            create table cf.collaborators
            (
                user_id uuid not null,
                report_id uuid not null,
                FOREIGN KEY (user_id) REFERENCES cf.users (id),
                FOREIGN KEY (report_id) REFERENCES cf.reports (id)
            );
        ";

        #endregion
        
        public override void Apply() => Database.ExecuteNonQuery( Script );
    }
}