namespace MultiplayerWebChess.ChessLogic
{
    public class BoardConstants
    {
        public const char EMPTY_FIELD = 'E';

        public const char WHITE_PAWN = 'P';
        public const char WHITE_ROOK = 'R';
        public const char WHITE_KNIGHT = 'N';
        public const char WHITE_BISHOP = 'B';
        public const char WHITE_QUEEN = 'Q';
        public const char WHITE_KING = 'K';

        public const char BLACK_PAWN = 'p';
        public const char BLACK_ROOK = 'r';
        public const char BLACK_KNIGHT = 'n';
        public const char BLACK_BISHOP = 'b';
        public const char BLACK_QUEEN = 'q';
        public const char BLACK_KING = 'k';

        public const string INITIAL_BOARD = "RNBQKBNRPPPPPPPPEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEpppppppprnbqkbnr";
        public const string INITIAL_WHITE_KING_POSITION = "E1";
        public const string INITIAL_BLACK_KING_POSITION = "E8";
    }
}