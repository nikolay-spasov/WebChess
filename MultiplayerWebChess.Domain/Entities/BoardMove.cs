using System;

namespace MultiplayerWebChess.Domain.Entities
{
    public class BoardMove
    {
        public int Id { get; set; }

        public string MoveContent { get; set; }

        public DateTime DateTimePlayed { get; set; }
    }
}
