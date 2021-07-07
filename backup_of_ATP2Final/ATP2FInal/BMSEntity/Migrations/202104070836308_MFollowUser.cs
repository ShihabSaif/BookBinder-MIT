namespace BMSEntity.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class MFollowUser : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.FollowUsers",
                c => new
                    {
                        FollowUserId = c.Int(nullable: false, identity: true),
                        MainUserId = c.Int(nullable: false),
                        GuestUserId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.FollowUserId);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.FollowUsers");
        }
    }
}
