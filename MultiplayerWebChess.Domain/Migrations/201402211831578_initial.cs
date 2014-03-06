namespace MultiplayerWebChess.Domain.Migrations
{
    using System.Data.Entity.Migrations;
    
    public partial class initial : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.BoardMoves",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        MoveContent = c.String(),
                        Game_Id = c.Guid(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Games", t => t.Game_Id)
                .Index(t => t.Game_Id);
            
            CreateTable(
                "dbo.Games",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        HostId = c.Int(nullable: false),
                        WhitePlayerId = c.Int(),
                        BlackPlayerId = c.Int(),
                        Description = c.String(),
                        DateCreated = c.DateTime(nullable: false),
                        IsWhiteTurn = c.Boolean(nullable: false),
                        GameState = c.Int(nullable: false),
                        BoardContent = c.String(maxLength: 64),
                        WhiteCanCastleKingSide = c.Boolean(nullable: false),
                        WhiteCanCastleQueenSide = c.Boolean(nullable: false),
                        BlackCanCastleKingSide = c.Boolean(nullable: false),
                        BlackCanCastleQueenSide = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.UserProfile",
                c => new
                    {
                        UserId = c.Int(nullable: false, identity: true),
                        UserName = c.String(maxLength: 80),
                    })
                .PrimaryKey(t => t.UserId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.BoardMoves", "Game_Id", "dbo.Games");
            DropIndex("dbo.BoardMoves", new[] { "Game_Id" });
            DropTable("dbo.UserProfile");
            DropTable("dbo.Games");
            DropTable("dbo.BoardMoves");
        }
    }
}
