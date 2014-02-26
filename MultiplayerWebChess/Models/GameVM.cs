using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MultiplayerWebChess.Models
{
    public class GameVM
    {
        public string GameId { get; set; }
        public bool Flipped { get; set; }
        public bool IsWhite { get; set; }

        public string FlippedHtmlClass
        {
            get { return this.Flipped ? "flipped-board" : ""; }
        }

        public string FieldId(int row, int col)
        {
            return string.Format("{0}{1}", (char)(col + 96), 9 - row);
        }

        public string FieldClass(int row, int col)
        {
            return ((9 - row - 1) * 8 + (col - 1)).ToString();
        }

        public string BlackOrWhiteFieldClass(int row, int col)
        {
            if ((row % 2 == 0 && col % 2 == 1) || (row % 2 == 1 && col % 2 == 0))
            {
                return "black-field";
            }
            else
            {
                return "white-field";
            }
        }
    }
}