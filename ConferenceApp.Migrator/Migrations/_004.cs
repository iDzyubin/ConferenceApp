using ThinkingHome.Migrator.Framework;

namespace ConferenceApp.Migrator.Migrations
{
    [Migration(4)]
    public class _004 : Migration
    {
        #region Script

        private string Script => @"
            alter table cf.reports
            add column user_id uuid not null,
            add foreign key (user_id) references cf.users(id);
        ";

        #endregion
        
        public override void Apply() => Database.ExecuteNonQuery(Script);
    }
}