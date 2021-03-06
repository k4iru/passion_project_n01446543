namespace card_app.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class dbsets : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.UserxDecks",
                c => new
                    {
                        UserxDeckId = c.Int(nullable: false, identity: true),
                        UserId = c.String(maxLength: 128),
                        DeckId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.UserxDeckId)
                .ForeignKey("dbo.Decks", t => t.DeckId, cascadeDelete: true)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId)
                .Index(t => t.UserId)
                .Index(t => t.DeckId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.UserxDecks", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.UserxDecks", "DeckId", "dbo.Decks");
            DropIndex("dbo.UserxDecks", new[] { "DeckId" });
            DropIndex("dbo.UserxDecks", new[] { "UserId" });
            DropTable("dbo.UserxDecks");
        }
    }
}
