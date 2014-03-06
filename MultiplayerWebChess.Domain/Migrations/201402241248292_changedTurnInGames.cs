namespace MultiplayerWebChess.Domain.Migrations
{
    using System.Data.Entity.Migrations;
    
    public partial class changedTurnInGames : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Games", "Turn", c => c.Int(nullable: false));
            DropColumn("dbo.Games", "IsWhiteTurn");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Games", "IsWhiteTurn", c => c.Boolean(nullable: false));
            DropColumn("dbo.Games", "Turn");
        }
    }
}
