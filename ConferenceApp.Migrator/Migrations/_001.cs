using ThinkingHome.Migrator.Framework;

namespace ConferenceApp.Migrator.Migrations
{
    [Migration( 1 )]
    public class _001 : Migration
    {
        #region Script

        private string Script => @"
            create schema if not exists cf;

            create table cf.users
            (
                id           uuid              NOT NULL PRIMARY KEY,
                first_name   character varying NOT NULL,
                middle_name  character varying,
                last_name    character varying NOT NULL,
                degree       integer           NOT NULL,
                organization character varying NOT NULL,
                address      character varying NOT NULL,
                phone        character varying NOT NULL,
                fax          character varying,
                email        character varying NOT NULL
            );
            
            CREATE TABLE cf.requests
            (
                id           uuid    NOT NULL PRIMARY KEY,
                owner_id     uuid    NOT NULL,
                status       integer NOT NULL DEFAULT 0,
                FOREIGN KEY (owner_id) REFERENCES cf.users (id)
            );
        
            CREATE TABLE cf.reports
            (
                id          uuid              NOT NULL PRIMARY KEY,
                title       character varying NOT NULL,
                request_id  uuid              NOT NULL,
                report_type integer           NOT NULL,
                file_name   character varying NOT NULL,
                path        character varying NOT NULL,
                status      integer           NOT NULL DEFAULT 0,
                FOREIGN KEY (request_id) REFERENCES cf.requests (id)
            );

            CREATE TABLE cf.collaborators
            (
                id           uuid              NOT NULL,
                first_name   character varying NOT NULL,
                middle_name  character varying,
                last_name    character varying NOT NULL,
                report_id    uuid              NOT NULL,
                FOREIGN KEY (report_id) REFERENCES cf.reports (id)
            );

            create table cf.admins
            (
                id       uuid              not null primary key,
                login    character varying not null, 
                password character varying not null
            );
        ";

        #endregion


        public override void Apply() => Database.ExecuteNonQuery( Script );
    }
}