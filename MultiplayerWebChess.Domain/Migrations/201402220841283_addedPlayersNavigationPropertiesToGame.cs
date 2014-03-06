namespace MultiplayerWebChess.Domain.Migrations
{
    using System.Data.Entity.Migrations;
    
    public partial class addedPlayersNavigationPropertiesToGame : DbMigration
    {
        public override void Up()
        {
            CreateIndex("dbo.Games", "BlackPlayerId");
            CreateIndex("dbo.Games", "WhitePlayerId");
            AddForeignKey("dbo.Games", "BlackPlayerId", "dbo.UserProfile", "UserId");
            AddForeignKey("dbo.Games", "WhitePlayerId", "dbo.UserProfile", "UserId");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Games", "WhitePlayerId", "dbo.UserProfile");
            DropForeignKey("dbo.Games", "BlackPlayerId", "dbo.UserProfile");
            DropIndex("dbo.Games", new[] { "WhitePlayerId" });
            DropIndex("dbo.Games", new[] { "BlackPlayerId" });
        }
    }
}
