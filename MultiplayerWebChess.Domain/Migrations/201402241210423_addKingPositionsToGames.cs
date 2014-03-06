namespace MultiplayerWebChess.Domain.Migrations
{
    using System.Data.Entity.Migrations;
    
    public partial class addKingPositionsToGames : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Games", "WhiteKingPosition", c => c.String(maxLength: 2));
            AddColumn("dbo.Games", "BlackKingPosition", c => c.String(maxLength: 2));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Games", "BlackKingPosition");
            DropColumn("dbo.Games", "WhiteKingPosition");
        }
    }
}
