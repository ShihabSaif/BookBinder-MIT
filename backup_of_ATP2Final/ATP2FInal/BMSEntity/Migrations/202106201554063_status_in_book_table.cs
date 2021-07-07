namespace BMSEntity.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class status_in_book_table : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Books", "status", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Books", "status");
        }
    }
}
