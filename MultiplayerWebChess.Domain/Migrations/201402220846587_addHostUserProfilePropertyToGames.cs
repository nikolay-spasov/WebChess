namespace MultiplayerWebChess.Domain.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addHostUserProfilePropertyToGames : DbMigration
    {
        public override void Up()
        {
            CreateIndex("dbo.Games", "HostId");
            AddForeignKey("dbo.Games", "HostId", "dbo.UserProfile", "UserId", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Games", "HostId", "dbo.UserProfile");
            DropIndex("dbo.Games", new[] { "HostId" });
        }
    }
}
