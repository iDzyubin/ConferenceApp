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

            create table cf.requests
            (
                id           uuid    NOT NULL PRIMARY KEY,
                owner_id     uuid    NOT NULL,
                status       integer NOT NULL DEFAULT 0,
                FOREIGN KEY (owner_id) REFERENCES cf.users (id)
            );

            create table cf.reports
            (
                id          uuid              NOT NULL PRIMARY KEY,
                title       character varying NOT NULL,
                request_id  uuid              NOT NULL,
                report_type integer           NOT NULL,
                path        character varying NOT NULL,
                status      integer           NOT NULL DEFAULT 0,
                FOREIGN KEY (request_id) REFERENCES cf.requests (id)
            );

            create table cf.collaborators
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

            INSERT INTO cf.admins(id, login, password)
	        VALUES ('638f6125-caf1-4a2a-b1c8-a8580ab6bb2e', 'admin@admin.com', 'password123');
        ";

        #endregion


        public override void Apply() => Database.ExecuteNonQuery( Script );
    }
}