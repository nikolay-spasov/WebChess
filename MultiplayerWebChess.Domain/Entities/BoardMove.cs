using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MultiplayerWebChess.Domain.Entities
{
    public class BoardMove
    {
        public int Id { get; set; }

        public string MoveContent { get; set; }

        public DateTime DateTimePlayed { get; set; }
    }
}
