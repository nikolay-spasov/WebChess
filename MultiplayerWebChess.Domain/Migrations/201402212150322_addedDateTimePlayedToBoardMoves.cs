namespace MultiplayerWebChess.Domain.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addedDateTimePlayedToBoardMoves : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.BoardMoves", "DateTimePlayed", c => c.DateTime(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.BoardMoves", "DateTimePlayed");
        }
    }
}
