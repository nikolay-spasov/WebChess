using MultiplayerWebChess.ChessLogic;
using MultiplayerWebChess.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace MultiplayerWebChess.ChessLogic
{
    public class DefaultMoveExecutor : IMoveExecutor
    {
        private const int WHITE_PAWN_INITIAL_ROW = 1;
        private const int BLACK_PAWN_INITIAL_ROW = 6;

        private static readonly int[] knightRows = { -1, -2, -2, -1, 1, 2, 2, 1 };
        private static readonly int[] knightCols = { -2, -1, 1, 2, 2, 1, -1, -2 };

        private BoardModel newState;
        private BoardModel oldState;
        private FieldPosition source;
        private FieldPosition dest;
        private FieldPosition lastMoveSource;
        private FieldPosition lastMoveDest;
        private char sourcePiece;

        public DefaultMoveExecutor(string board, string newMove, Turn turn,
            GameState gameState, string whiteKingPosition, string blackKingPosition, string lastMove,
            bool whiteCanCastleKingSide, bool whiteCanCastleQueenSide, bool blackCanCastleKingSide,
            bool blackCanCastleQueenSide)
        {
            FieldPosition whiteKing = FieldPosition.ParsePosition(whiteKingPosition);
            FieldPosition blackKing = FieldPosition.ParsePosition(blackKingPosition);

            this.oldState = new BoardModel(GetBoard(board), turn, gameState, whiteKing, blackKing,
                whiteCanCastleKingSide, whiteCanCastleQueenSide, blackCanCastleKingSide, blackCanCastleQueenSide);

            this.newState = new BoardModel(GetBoard(board), turn, gameState, whiteKing, blackKing,
                whiteCanCastleKingSide, whiteCanCastleQueenSide, blackCanCastleKingSide, blackCanCastleQueenSide);

            // Parse new move
            string[] splt = newMove.Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries);
            this.source = FieldPosition.ParsePosition(splt[0]);
            this.dest = FieldPosition.ParsePosition(splt[1]);

            if (lastMove != null)
            {
                // Parse last move
                splt = lastMove.Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries);
                this.lastMoveSource = FieldPosition.ParsePosition(splt[0]);
                this.lastMoveDest = FieldPosition.ParsePosition(splt[1]);
            }

            this.sourcePiece = this.newState.Board[source.Row, source.Col];
        }

        private static char[,] GetBoard(string board)
        {
            char[,] b = new char[8, 8];
            for (int row = 0; row < 8; row++)
            {
                for (int col = 0; col < 8; col++)
                {
                    b[row, col] = board[row * 8 + col];
                }
            }

            return b;
        }

        public bool IsNewMoveValid()
        {
            if (!IsPreValid())
            {
                return false;
            }

            if (CanMoveThere())
            {
                // Apply new move to the board.
                newState.Board[dest.Row, dest.Col] = newState.Board[source.Row, source.Col];
                newState.Board[source.Row, source.Col] = BoardConstants.EMPTY_FIELD;

                FieldPosition currentKingPosition = (newState.Turn == Turn.White)
                    ? newState.WhiteKingPosition : newState.BlackKingPosition;
                if (IsFieldUnderAttack(currentKingPosition.Row, currentKingPosition.Col))
                {
                    // Current player's king is under attack after applying the move.
                    RollbackChanges();
                    return false;
                }

                UpdateCastleStates();

                ChangePlayerTurn();

                return true;
            }

            return false;
        }

        private void UpdateCastleStates()
        {
            if (sourcePiece == BoardConstants.WHITE_ROOK)
            {
                if (source.Col == 0)
                {
                    this.newState.WhiteCanCastleQueenSide = false;
                }
                else if (source.Col == 7)
                {
                    this.newState.WhiteCanCastleKingSide = false;
                }
            }
            else if (sourcePiece == BoardConstants.BLACK_ROOK)
            {
                if (source.Col == 0)
                {
                    this.newState.BlackCanCastleQueenSide = false;
                }
                else if (source.Col == 7)
                {
                    this.newState.BlackCanCastleKingSide = false;
                }
            }
        }

        private void RollbackChanges()
        {
            this.newState = this.oldState;
        }

        private bool IsPreValid()
        {
            if (source.Row < 0 || source.Col < 0 || source.Row >= 8 || source.Col >= 8 ||
                dest.Row < 0 || dest.Col < 0 || dest.Row >= 8 || dest.Col >= 8)
            {
                // Player is trying to move outside of the board.
                return false;
            }

            if (source.Row == dest.Row && source.Col == dest.Col)
            {
                // Player is trying to move piece at the same position.
                return false;
            }

            if (sourcePiece == BoardConstants.EMPTY_FIELD)
            {
                // Player is trying to move empty piece.
                return false;
            }

            char destPiece = this.newState.Board[dest.Row, dest.Col];
            if ((Char.IsUpper(sourcePiece) && Char.IsUpper(destPiece) && destPiece != BoardConstants.EMPTY_FIELD) ||
                (Char.IsLower(sourcePiece) && Char.IsLower(destPiece)))
            {
                // Player is trying to take his own piece.
                return false;
            }

            return true;
        }

        private bool CanMoveThere()
        {
            switch (sourcePiece)
            {
                case BoardConstants.WHITE_PAWN:
                case BoardConstants.BLACK_PAWN:
                    {
                        return IsValidPawnMove();
                    }
                case BoardConstants.WHITE_ROOK:
                case BoardConstants.BLACK_ROOK:
                    {
                        return IsValidLineMove();
                    }
                case BoardConstants.WHITE_KNIGHT:
                case BoardConstants.BLACK_KNIGHT:
                    {
                        return IsValidKnightMove();
                    }
                case BoardConstants.WHITE_BISHOP:
                case BoardConstants.BLACK_BISHOP:
                    {
                        return IsValidDiagonalMove();
                    }
                case BoardConstants.WHITE_QUEEN:
                case BoardConstants.BLACK_QUEEN:
                    {
                        return IsValidQueenMove();
                    }
                case BoardConstants.WHITE_KING:
                case BoardConstants.BLACK_KING:
                    {
                        return IsValidKingMove();
                    }
                default:
                    {
                        return false;
                    }
            }
        }

        private bool IsValidPawnMove()
        {
            if ((newState.Turn == Turn.White && source.Row < dest.Row) ||
                (newState.Turn == Turn.Black && source.Row > dest.Row))
            {
                bool sameCol = source.Col == dest.Col;
                int fieldsForward = Math.Abs(source.Row - dest.Row);

                if (sameCol)
                {
                    int initalRow = (newState.Turn == Turn.White) ? WHITE_PAWN_INITIAL_ROW : BLACK_PAWN_INITIAL_ROW;

                    // Player is moving pawn one field forward.
                    if (fieldsForward == 1 && newState.Board[dest.Row, dest.Col] == BoardConstants.EMPTY_FIELD)
                    {
                        return true;
                    }
                    // Player is moving pawn two fields forward.
                    else if (source.Row == initalRow && fieldsForward == 2 &&
                        newState.Board[dest.Row, dest.Col] == BoardConstants.EMPTY_FIELD &&
                        ((newState.Turn == Turn.White && newState.Board[source.Row + 1, source.Col] == BoardConstants.EMPTY_FIELD) ||
                        (newState.Turn == Turn.Black && newState.Board[source.Row - 1, source.Col] == BoardConstants.EMPTY_FIELD)))
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
                // Player is taking a piece.
                else if (fieldsForward == 1 && (source.Col + 1 == dest.Col || source.Col - 1 == dest.Col))
                {
                    int enPassantRow = (newState.Turn == Turn.White) ? 4 : 3;
                    int lastMoveInitialRow = (newState.Turn == Turn.White) ? BLACK_PAWN_INITIAL_ROW : WHITE_PAWN_INITIAL_ROW;
                    int lastMoveDestRow = (newState.Turn == Turn.White) ? BLACK_PAWN_INITIAL_ROW - 2 : WHITE_PAWN_INITIAL_ROW + 2;
                    char lastMovePiece = newState.Board[lastMoveDest.Row, lastMoveDest.Col];
                    bool isLastMovedPieceEnemyPawn = lastMovePiece == BoardConstants.WHITE_PAWN ||
                        lastMovePiece == BoardConstants.BLACK_PAWN;
                    bool isEnPassantPossible = isLastMovedPieceEnemyPawn &&
                        lastMoveSource.Row == lastMoveInitialRow && lastMoveDest.Row == lastMoveDestRow;

                    if (newState.Board[dest.Row, dest.Col] != BoardConstants.EMPTY_FIELD)
                    {
                        return true;
                    }
                    // En passant rule.
                    else if (isEnPassantPossible && isLastMovedPieceEnemyPawn &&
                        source.Row == enPassantRow && dest.Col == lastMoveSource.Col)
                    {
                        //changes.Enqueue(new BoardChange(source.Row, dest.Col, board[source.Row, dest.Col]));
                        newState.Board[source.Row, dest.Col] = BoardConstants.EMPTY_FIELD;
                        return true;
                    }
                }
            }

            return false;
        }

        private bool IsValidLineMove()
        {
            // Horizontal
            if (source.Row == dest.Row)
            {
                int startCol = Math.Min(source.Col, dest.Col);
                int endCol = Math.Max(source.Col, dest.Col);

                for (int i = startCol + 1; i < endCol; i++)
                {
                    if (newState.Board[source.Row, i] != BoardConstants.EMPTY_FIELD)
                    {
                        return false;
                    }
                }
            }
            // Vertical
            else if (source.Col == dest.Col)
            {
                int startRow = Math.Min(source.Row, dest.Row);
                int endRow = Math.Max(source.Row, dest.Row);

                for (int i = startRow + 1; i < endRow; i++)
                {
                    if (newState.Board[i, source.Col] != BoardConstants.EMPTY_FIELD)
                    {
                        return false;
                    }
                }
            }
            else
            {
                return false;
            }

            return true;
        }

        private bool IsValidKnightMove()
        {
            for (int i = 0; i < knightRows.Length; i++)
            {
                int currentRow = source.Row + knightRows[i];
                int currentCol = source.Col + knightCols[i];
                if (currentRow >= 0 && currentRow < 8 && currentCol >= 0 && currentCol < 8 &&
                    currentRow == dest.Row && currentCol == dest.Col)
                {
                    return true;
                }
            }

            return false;
        }

        private bool IsValidDiagonalMove()
        {
            if (source.Row == dest.Row || source.Col == dest.Col)
            {
                return false;
            }

            int currentRow = source.Row;
            int currentCol = source.Col;
            int row = 0;
            int col = 0;

            if (source.Row < dest.Row)
            {
                row = 1;
                if (source.Col < dest.Col)
                {
                    col = 1;
                }
                else
                {
                    col = -1;
                }
            }
            else if (source.Row > dest.Row)
            {
                row = -1;
                if (source.Col < dest.Col)
                {
                    col = 1;
                }
                else
                {
                    col = -1;
                }
            }

            for (int i = 0; i < 8; i++)
            {
                currentRow += row;
                currentCol += col;

                if (currentRow == dest.Row && currentCol == dest.Col)
                {
                    return true;
                }

                if (newState.Board[currentRow, currentCol] != BoardConstants.EMPTY_FIELD)
                {
                    return false;
                }
            }

            return false;
        }

        private bool IsValidQueenMove()
        {
            return IsValidLineMove() || IsValidDiagonalMove();
        }

        private bool IsValidKingMove()
        {
            // TODO: Faster algorithm with Math.Abs
            for (int row = -1; row <= 1; row++)
            {
                for (int col = -1; col <= 1; col++)
                {
                    if (row == 0 && col == 0)
                    {
                        continue;
                    }
                    if (source.Row + row == dest.Row && source.Col + col == dest.Col)
                    {
                        if (newState.Turn == Turn.White)
                        {
                            newState.WhiteKingPosition.Row = dest.Row;
                            newState.WhiteKingPosition.Col = dest.Col;
                            newState.WhiteCanCastleKingSide = false;
                            newState.WhiteCanCastleQueenSide = false;
                        }
                        else
                        {
                            newState.BlackKingPosition.Row = dest.Row;
                            newState.BlackKingPosition.Col = dest.Col;
                            newState.BlackCanCastleKingSide = false;
                            newState.BlackCanCastleQueenSide = false;
                        }
                        return true;
                    }
                }
            }

            // Castle
            if (source.Row == dest.Row)
            {
                if (source.Col + 2 == dest.Col &&
                    ((newState.Turn == Turn.White && newState.WhiteCanCastleKingSide) ||
                    (newState.Turn == Turn.Black && newState.BlackCanCastleKingSide)))
                {
                    if (newState.Board[source.Row, source.Col + 1] == BoardConstants.EMPTY_FIELD &&
                        newState.Board[source.Row, source.Col + 2] == BoardConstants.EMPTY_FIELD &&
                        !IsFieldUnderAttack(source.Row, source.Col) && !IsFieldUnderAttack(source.Row, source.Col + 1) &&
                        !IsFieldUnderAttack(source.Row, source.Col + 2))
                    {
                        if (newState.Turn == Turn.White)
                        {
                            newState.Board[0, 5] = BoardConstants.WHITE_ROOK;
                            newState.Board[0, 7] = BoardConstants.EMPTY_FIELD;
                            newState.WhiteKingPosition.Col = 6;
                            newState.WhiteCanCastleKingSide = false;
                            newState.WhiteCanCastleQueenSide = false;
                            return true;
                        }
                        else
                        {
                            newState.Board[7, 5] = BoardConstants.BLACK_ROOK;
                            newState.Board[7, 7] = BoardConstants.EMPTY_FIELD;
                            newState.BlackKingPosition.Col = 6;
                            newState.BlackCanCastleKingSide = false;
                            newState.BlackCanCastleQueenSide = false;
                            return true;
                        }
                    }
                }
                else if (source.Col - 2 == dest.Col &&
                    ((newState.Turn == Turn.White && newState.WhiteCanCastleQueenSide) ||
                    (newState.Turn == Turn.Black && newState.BlackCanCastleQueenSide)))
                {
                    if (newState.Board[source.Row, source.Col - 1] == BoardConstants.EMPTY_FIELD &&
                        newState.Board[source.Row, source.Col - 2] == BoardConstants.EMPTY_FIELD &&
                        !IsFieldUnderAttack(source.Row, source.Col) && !IsFieldUnderAttack(source.Row, source.Col - 1) &&
                        !IsFieldUnderAttack(source.Row, source.Col - 2))
                    {
                        if (newState.Turn == Turn.White)
                        {
                            newState.Board[0, 3] = BoardConstants.WHITE_ROOK;
                            newState.Board[0, 0] = BoardConstants.EMPTY_FIELD;
                            newState.WhiteKingPosition.Col = 2;
                            newState.WhiteCanCastleKingSide = false;
                            newState.WhiteCanCastleQueenSide = false;
                            return true;
                        }
                        else
                        {
                            newState.Board[7, 3] = BoardConstants.BLACK_ROOK;
                            newState.Board[7, 0] = BoardConstants.EMPTY_FIELD;
                            newState.BlackKingPosition.Col = 2;
                            newState.BlackCanCastleKingSide = false;
                            newState.BlackCanCastleQueenSide = false;
                            return true;
                        }
                    }
                }
            }

            return false;
        }

        private void ChangePlayerTurn()
        {
            newState.Turn = (newState.Turn == Turn.White) ? Turn.Black : Turn.White;
        }

        private bool IsFieldUnderAttack(int row, int col)
        {
            char opponentPawn = (newState.Turn == Turn.White)
                ? BoardConstants.BLACK_PAWN : BoardConstants.WHITE_PAWN;

            // Pawn
            int pawnRow = row + ((newState.Turn == Turn.White) ? 1 : -1);
            if ((pawnRow >= 0 && pawnRow < 8) &&
                ((col - 1 >= 0 && newState.Board[pawnRow, col - 1] == opponentPawn) ||
                (col + 1 < 8 && newState.Board[pawnRow, col + 1] == opponentPawn)))
            {
                return true;
            }

            char opponentKnight = (newState.Turn == Turn.White)
                ? BoardConstants.BLACK_KNIGHT : BoardConstants.WHITE_KNIGHT;

            // Knight
            for (int i = 0; i < knightRows.Length; i++)
            {
                int r = row + knightRows[i];
                int c = col + knightCols[i];
                if (r >= 0 && r < 8 && c >= 0 && c < 8 && newState.Board[r, c] == opponentKnight)
                {
                    return true;
                }
            }

            char opponentQueen = (newState.Turn == Turn.White)
                ? BoardConstants.BLACK_QUEEN : BoardConstants.WHITE_QUEEN;

            char opponentRook = (newState.Turn == Turn.White)
                ? BoardConstants.BLACK_ROOK : BoardConstants.WHITE_ROOK;

            // Lines
            // Up
            for (int i = 1; i < 8; i++)
            {
                int r = row - i;
                int c = col;
                if (r >= 0 && newState.Board[r, c] != BoardConstants.EMPTY_FIELD)
                {
                    if (newState.Board[r, c] == opponentRook || newState.Board[r, c] == opponentQueen)
                    {
                        return true;
                    }
                    else
                    {
                        break;
                    }
                }
            }
            // Down
            for (int i = 1; i < 8; i++)
            {
                int r = row + i;
                int c = col;
                if (r < 8 && newState.Board[r, c] != BoardConstants.EMPTY_FIELD)
                {
                    if (newState.Board[r, c] == opponentRook || newState.Board[r, c] == opponentQueen)
                    {
                        return true;
                    }
                    else
                    {
                        break;
                    }
                }
            }
            // Left
            for (int i = 1; i < 8; i++)
            {
                int r = row;
                int c = col - i;
                if (c >= 0 && newState.Board[r, c] != BoardConstants.EMPTY_FIELD)
                {
                    if (newState.Board[r, c] == opponentRook || newState.Board[r, c] == opponentQueen)
                    {
                        return true;
                    }
                    else
                    {
                        break;
                    }
                }
            }
            // Right
            for (int i = 1; i < 8; i++)
            {
                int r = row;
                int c = col + i;
                if (c < 8 && newState.Board[r, c] != BoardConstants.EMPTY_FIELD)
                {
                    if (newState.Board[r, c] == opponentRook || newState.Board[r, c] == opponentQueen)
                    {
                        return true;
                    }
                    else
                    {
                        break;
                    }
                }
            }

            char opponentBishop = (newState.Turn == Turn.White)
                ? BoardConstants.BLACK_BISHOP : BoardConstants.WHITE_BISHOP;
            // Diagonals
            // Up-Left
            for (int i = 1; i < 8; i++)
            {
                int r = row - i;
                int c = col - i;
                if (r >= 0 && c >= 0 && newState.Board[r, c] != BoardConstants.EMPTY_FIELD)
                {
                    if (newState.Board[r, c] == opponentBishop || newState.Board[r, c] == opponentQueen)
                    {
                        return true;
                    }
                    else
                    {
                        break;
                    }
                }
            }
            // Up-Right
            for (int i = 1; i < 8; i++)
            {
                int r = row - i;
                int c = col + i;
                if (r >= 0 && c < 8 && newState.Board[r, c] != BoardConstants.EMPTY_FIELD)
                {
                    if (newState.Board[r, c] == opponentBishop || newState.Board[r, c] == opponentQueen)
                    {
                        return true;
                    }
                    else
                    {
                        break;
                    }
                }
            }
            // Down-Left
            for (int i = 1; i < 8; i++)
            {
                int r = row + i;
                int c = col - i;
                if (r < 8 && c >= 0 && newState.Board[r, c] != BoardConstants.EMPTY_FIELD)
                {
                    if (newState.Board[r, c] == opponentBishop || newState.Board[r, c] == opponentQueen)
                    {
                        return true;
                    }
                    else
                    {
                        break;
                    }
                }
            }
            // Down-Right
            for (int i = 1; i < 8; i++)
            {
                int r = row + i;
                int c = col + i;
                if (r < 8 && c < 8 && newState.Board[r, c] != BoardConstants.EMPTY_FIELD)
                {
                    if (newState.Board[r, c] == opponentBishop || newState.Board[r, c] == opponentQueen)
                    {
                        return true;
                    }
                    else
                    {
                        break;
                    }
                }
            }

            return false;
        }

        public BoardModel NewBoardModelState
        {
            get { return this.newState; }
        }
    }
}