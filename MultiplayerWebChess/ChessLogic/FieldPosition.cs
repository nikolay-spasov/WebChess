using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MultiplayerWebChess.ChessLogic
{
    public class FieldPosition
    {
        public FieldPosition(int row, int col)
        {
            Row = row;
            Col = col;
        }

        public int Row { get; set; }

        public int Col { get; set; }

        public static FieldPosition ParsePosition(string positionToParse)
        {
            return new FieldPosition((int)(positionToParse[1] - '1'),
                (int)(positionToParse[0] - 'A'));
        }

        public override string ToString()
        {
            char row = (char)('1' + Row);
            char col = (char)('A' + Col);
            return col.ToString() + row.ToString();
        }
    }
}