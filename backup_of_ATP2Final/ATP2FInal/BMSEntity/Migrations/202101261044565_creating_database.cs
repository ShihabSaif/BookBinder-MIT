namespace BMSEntity.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class creating_database : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Books",
                c => new
                    {
                        BookId = c.Int(nullable: false, identity: true),
                        title = c.String(),
                        author = c.String(),
                        category = c.String(),
                        description = c.String(),
                        image_link = c.String(),
                        review = c.Double(nullable: false),
                    })
                .PrimaryKey(t => t.BookId);
            
            CreateTable(
                "dbo.UserBooks",
                c => new
                    {
                        userBookId = c.Int(nullable: false, identity: true),
                        userId = c.Int(nullable: false),
                        BookId = c.Int(nullable: false),
                        readStatus = c.Int(nullable: false),
                        Rating = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.userBookId);
            
            CreateTable(
                "dbo.Users",
                c => new
                    {
                        userId = c.Int(nullable: false, identity: true),
                        user_name = c.String(),
                        email = c.String(),
                        password = c.String(),
                        image_link = c.String(),
                        BookCount = c.Int(nullable: false),
                        AverageRating = c.Double(nullable: false),
                    })
                .PrimaryKey(t => t.userId);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Users");
            DropTable("dbo.UserBooks");
            DropTable("dbo.Books");
        }
    }
}
