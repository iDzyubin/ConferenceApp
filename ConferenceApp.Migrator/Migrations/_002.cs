using ThinkingHome.Migrator.Framework;

namespace ConferenceApp.Migrator.Migrations
{
    [Migration( 2 )]
    public class _002 : Migration
    {
        #region Scripts

        private string ApplyScript => @"
            create table cf.admins
            (
                id       uuid              not null primary key,
                login    character varying not null, 
                password character varying not null
            );
        ";

        private string RevertScript => @"
            drop table cf.admins;
        ";
        
        #endregion


        public override void Apply() 
            => Database.ExecuteNonQuery( ApplyScript );

        
        public override void Revert() 
            => Database.ExecuteNonQuery( RevertScript );
    }
}