using MultiplayerWebChess.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace MultiplayerWebChess.ChessLogic
{
    public class BoardModel
    {
        public char[,] Board { get; set; }
        public Turn Turn { get; set; }
        public GameState GameState { get; set; }
        public FieldPosition WhiteKingPosition { get; set; }
        public FieldPosition BlackKingPosition { get; set; }
        public bool WhiteCanCastleKingSide { get; set; }
        public bool WhiteCanCastleQueenSide { get; set; }
        public bool BlackCanCastleKingSide { get; set; }
        public bool BlackCanCastleQueenSide { get; set; }

        public BoardModel(char[,] board, Turn turn, GameState gameState,
            FieldPosition whiteKingPosition, FieldPosition blackKingPosition,
            bool whiteCanCastleKingSide, bool whiteCanCastleQueenSide,
            bool blackCanCastleKingSide, bool blackCanCastleQueenSide)
        {
            this.Board = board;
            this.Turn = turn;
            this.GameState = gameState;
            this.WhiteKingPosition = whiteKingPosition;
            this.BlackKingPosition = blackKingPosition;
            this.WhiteCanCastleKingSide = whiteCanCastleKingSide;
            this.WhiteCanCastleQueenSide = whiteCanCastleQueenSide;
            this.BlackCanCastleKingSide = blackCanCastleKingSide;
            this.BlackCanCastleQueenSide = blackCanCastleQueenSide;
        }

        public string GetBoardAsString()
        {
            StringBuilder result = new StringBuilder(64);
            for (int row = 0; row < 8; row++)
            {
                for (int col = 0; col < 8; col++)
                {
                    result.Append(this.Board[row, col]);
                }
            }

            return result.ToString();
        }
    }
}