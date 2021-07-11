namespace BMSEntity.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class date_time_added_in_userBook_model : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.UserBooks", "statusDate", c => c.DateTime(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.UserBooks", "statusDate");
        }
    }
}
