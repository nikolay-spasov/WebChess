using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MultiplayerWebChess.Domain.Entities
{
    public class Game
    {
        [Key]
        public Guid Id { get; set; }

        public string Description { get; set; }

        public DateTime DateCreated { get; set; }

        public Turn Turn { get; set; }

        public GameState GameState { get; set; }

        [StringLength(64)]
        public string BoardContent { get; set; }

        public bool WhiteCanCastleKingSide { get; set; }
        public bool WhiteCanCastleQueenSide { get; set; }
        public bool BlackCanCastleKingSide { get; set; }
        public bool BlackCanCastleQueenSide { get; set; }

        [StringLength(2)]
        public string WhiteKingPosition { get; set; }

        [StringLength(2)]
        public string BlackKingPosition { get; set; }

        [ForeignKey("Host")]
        public int HostId { get; set; }

        public virtual UserProfile Host { get; set; }

        [ForeignKey("WhitePlayer")]
        public int? WhitePlayerId { get; set; }

        public virtual UserProfile WhitePlayer { get; set; }

        [ForeignKey("BlackPlayer")]
        public int? BlackPlayerId { get; set; }

        public virtual UserProfile BlackPlayer { get; set; }

        public virtual IList<BoardMove> BoardMoves { get; set; }
    }
}
