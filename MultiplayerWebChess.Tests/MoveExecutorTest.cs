using Microsoft.VisualStudio.TestTools.UnitTesting;
using MultiplayerWebChess.ChessLogic;
using MultiplayerWebChess.Domain.Entities;

namespace ChessBrutality.WebApp.Tests
{
    [TestClass]
    public class MoveExecutorTest
    {
        private IMoveExecutor GetMoveExecutor(string board, string newMove, Turn whichTurn,
            GameState gameState = GameState.Playing, string whiteKingPosition = null, string blackKingPosition = null,
            string lastMove = null, bool whiteCanCastleKingSide = true,
            bool whiteCanCastleQueenSide = true, bool blackCanCastleKingSide = true,
            bool blackCanCastleQueenSide = true)
        {
            return new DefaultMoveExecutor(board, newMove, whichTurn, gameState, whiteKingPosition,
                blackKingPosition, lastMove, whiteCanCastleKingSide, whiteCanCastleQueenSide,
                blackCanCastleKingSide, blackCanCastleQueenSide);
        }

        [TestMethod]
        public void WhitePawn_CanMoveOneForward()
        {
            string board =
                "EEEEEEEK" +
                "PEEEEEEE" +
                "EEEEEEEk" +
                "EEEEEEEE" +
                "EEEEEEEE" +
                "EEEEEEEE" +
                "EEEEEEEE" +
                "EEEEEEEE";
            var executor = GetMoveExecutor(board, "A2;A3", Turn.White, GameState.Playing, "H1", "H3");

            var result = executor.IsNewMoveValid();

            Assert.AreEqual(true, result);
        }

        [TestMethod]
        public void WhitePawn_CanMoveTwoForward()
        {
            string board =
                "EEEEEEEK" +
                "PEEEEEEE" +
                "EEEEEEEk" +
                "EEEEEEEE" +
                "EEEEEEEE" +
                "EEEEEEEE" +
                "EEEEEEEE" +
                "EEEEEEEE";
            var executor = GetMoveExecutor(board, "A2;A4", Turn.White, GameState.Playing, "H1", "H3");

            var result = executor.IsNewMoveValid();

            Assert.AreEqual(true, result);
        }

        [TestMethod]
        public void WhitePawn_CannotMoveTwoForwardIfDoesNotStartFromInitialRow()
        {
            string board =
                "EEEEEEEK" +
                "EEEEEEEE" +
                "PEEEEEEk" +
                "EEEEEEEE" +
                "EEEEEEEE" +
                "EEEEEEEE" +
                "EEEEEEEE" +
                "EEEEEEEE";
            var executor = GetMoveExecutor(board, "A3;A5", Turn.White, GameState.Playing, "H1", "H3");

            var result = executor.IsNewMoveValid();

            Assert.AreEqual(false, result);
        }

        [TestMethod]
        public void WhitePawn_CannotMoveTwoForwardIfSomethingInTheWay()
        {
            string board =
               "EEEEEEEK" +
               "PEEEEEEE" +
               "pEEEEEEk" +
               "EEEEEEEE" +
               "EEEEEEEE" +
               "EEEEEEEE" +
               "EEEEEEEE" +
               "EEEEEEEE";
            var executor = GetMoveExecutor(board, "A2;A4", Turn.White, GameState.Playing,
                "H1", "H3", "A5;A3");

            var result = executor.IsNewMoveValid();

            Assert.AreEqual(false, result);
        }

        [TestMethod]
        public void WhitePawn_CanTakeAPiece()
        {
            string board =
                "EEEEEEEK" +
                "PEEEEEEE" +
                "EpEEEEEk" +
                "EEEEEEEE" +
                "EEEEEEEE" +
                "EEEEEEEE" +
                "EEEEEEEE" +
                "EEEEEEEE";
            var executor = GetMoveExecutor(board, "A2;B3", Turn.White, GameState.Playing,
                "H1", "H3", "A5;A3");

            var result = executor.IsNewMoveValid();

            Assert.AreEqual(true, result);
        }

        [TestMethod]
        public void WhitePawn_CannotMoveBackward()
        {
            string board =
                "EEEEEEEK" +
                "PEEEEEEE" +
                "EEEEEEEk" +
                "EEEEEEEE" +
                "EEEEEEEE" +
                "EEEEEEEE" +
                "EEEEEEEE" +
                "EEEEEEEE";
            var executor = GetMoveExecutor(board, "A2;A1", Turn.White, GameState.Playing,
                "H1", "H3", "A5;A3");

            var result = executor.IsNewMoveValid();

            Assert.AreEqual(false, result);
        }

        [TestMethod]
        public void WhitePawn_CannotMoveThreeForward()
        {
            string board =
                "EEEEEEEK" +
                "PEEEEEEE" +
                "EEEEEEEk" +
                "EEEEEEEE" +
                "EEEEEEEE" +
                "EEEEEEEE" +
                "EEEEEEEE" +
                "EEEEEEEE";
            var executor = GetMoveExecutor(board, "A2;A5", Turn.White, GameState.Playing,
                "H1", "H3", "A5;A3");

            var result = executor.IsNewMoveValid();

            Assert.AreEqual(false, result);
        }

        [TestMethod]
        public void WhitePawn_CannotTakeAPieceOneForward()
        {
            string board =
                "EEEEEEEK" +
                "PEEEEEEE" +
                "pEEEEEEk" +
                "EEEEEEEE" +
                "EEEEEEEE" +
                "EEEEEEEE" +
                "EEEEEEEE" +
                "EEEEEEEE";
            var executor = GetMoveExecutor(board, "A2;A3", Turn.White, GameState.Playing,
                "H1", "H3", "A5;A3");

            var result = executor.IsNewMoveValid();

            Assert.AreEqual(false, result);
        }

        [TestMethod]
        public void WhitePawn_CanTakeWithEnPassantRule()
        {
            string board =
                "EEEEEEEK" +
                "EEEEEEEE" +
                "EEEEEEEk" +
                "EEEEEEEE" +
                "pPEEEEEE" +
                "EEEEEEEE" +
                "EEEEEEEE" +
                "EEEEEEEE";
            var executor = GetMoveExecutor(board, "B5;A6", Turn.White, GameState.Playing,
                "H1", "H3", "A7;A5");

            var result = executor.IsNewMoveValid();

            Assert.AreEqual(true, result);
        }

        [TestMethod]
        public void WhitePawn_CannotTakeWithEnPassantRuleIfLastMoveIsNotDoubleEnemyPawnMove()
        {
            string board =
                "EEEEEEEK" +
                "EEEEEEEE" +
                "EEEEEEEk" +
                "EEEEEEEE" +
                "pPEEEEEE" +
                "EEEEEEEE" +
                "EEEEEEEE" +
                "EEEEEEEE";
            var excutor = GetMoveExecutor(board, "B5;A6", Turn.White, GameState.Playing,
                "H1", "H3", "A6;A5");

            var result = excutor.IsNewMoveValid();

            Assert.AreEqual(false, result);
        }

        [TestMethod]
        public void BlackPawn_CanMoveOneForward()
        {
            string board =
                "EEEEEEEK" +
                "EEEEEEEE" +
                "EEEEEEEk" +
                "EEEEEEEE" +
                "EEEEEEEE" +
                "EEEEEEEE" +
                "pEEEEEEE" +
                "EEEEEEEE";
            var executor = GetMoveExecutor(board, "A7;A6", Turn.Black, GameState.Playing,
                "H1", "H3", "A5;A3");

            var result = executor.IsNewMoveValid();

            Assert.AreEqual(true, result);
        }

        [TestMethod]
        public void BlackPawn_CanMoveTwoForward()
        {
            string board =
                "EEEEEEEK" +
                "EEEEEEEE" +
                "EEEEEEEk" +
                "EEEEEEEE" +
                "EEEEEEEE" +
                "EEEEEEEE" +
                "pEEEEEEE" +
                "EEEEEEEE";
            var executor = GetMoveExecutor(board, "A7;A5", Turn.Black, GameState.Playing,
                "H1", "H3", "A5;A3");

            var result = executor.IsNewMoveValid();

            Assert.AreEqual(true, result);
        }

        [TestMethod]
        public void BlackPawn_CanMoveBackward()
        {
            string board =
                "EEEEEEEK" +
                "EEEEEEEE" +
                "EEEEEEEk" +
                "EEEEEEEE" +
                "EEEEEEEE" +
                "EEEEEEEE" +
                "pEEEEEEE" +
                "EEEEEEEE";
            var executor = GetMoveExecutor(board, "A7;A8", Turn.Black, GameState.Playing,
                "H1", "H3", "A5;A3");

            var result = executor.IsNewMoveValid();

            Assert.AreEqual(false, result);
        }

        [TestMethod]
        public void BlackPawn_CannotTakeAPieceOneForward()
        {
            string board =
                "EEEEEEEK" +
                "EEEEEEEE" +
                "EEEEEEEk" +
                "EEEEEEEE" +
                "EEEEEEEE" +
                "PEEEEEEE" +
                "pEEEEEEE" +
                "EEEEEEEE";
            var executor = GetMoveExecutor(board, "A7;A6", Turn.Black, GameState.Playing,
                "H1", "H3", "A5;A3");

            var result = executor.IsNewMoveValid();

            Assert.AreEqual(false, result);
        }

        [TestMethod]
        public void BlackPawn_CannotMoveTwoForwardIfDoesNotStartFromInitialRow()
        {
            string board =
                "EEEEEEEK" +
                "EEEEEEEE" +
                "EEEEEEEk" +
                "EEEEEEEE" +
                "EEEEEEEE" +
                "pEEEEEEE" +
                "EEEEEEEE" +
                "EEEEEEEE";
            var executor = GetMoveExecutor(board, "A6;A4", Turn.Black, GameState.Playing,
                "H1", "H3", "A5;A3");

            var result = executor.IsNewMoveValid();

            Assert.AreEqual(false, result);
        }

        [TestMethod]
        public void BlackPawn_CannotMoveThreeForward()
        {
            string board =
                "EEEEEEEK" +
                "EEEEEEEE" +
                "EEEEEEEk" +
                "EEEEEEEE" +
                "EEEEEEEE" +
                "EEEEEEEE" +
                "pEEEEEEE" +
                "EEEEEEEE";
            var executor = GetMoveExecutor(board, "A7;A4", Turn.Black, GameState.Playing,
                "H1", "H3", "A5;A3");

            var result = executor.IsNewMoveValid();

            Assert.AreEqual(false, result);
        }

        [TestMethod]
        public void BlackPawn_CannotMoveTwoForwardIfSomethingInTheWay()
        {
            string board =
               "EEEEEEEK" +
               "EEEEEEEE" +
               "EEEEEEEk" +
               "EEEEEEEE" +
               "EEEEEEEE" +
               "PEEEEEEE" +
               "pEEEEEEE" +
               "EEEEEEEE";
            var executor = GetMoveExecutor(board, "A7;A5", Turn.Black, GameState.Playing,
                "H1", "H3", "A5;A3");

            var result = executor.IsNewMoveValid();

            Assert.AreEqual(false, result);
        }

        [TestMethod]
        public void BlackPawn_CanTakeAPiece()
        {
            string board =
                "EEEEEEEK" +
                "EEEEEEEE" +
                "EEEEEEEk" +
                "EEEEEEEE" +
                "EEEEEEEE" +
                "EPEEEEEE" +
                "pEEEEEEE" +
                "EEEEEEEE";
            var executor = GetMoveExecutor(board, "A7;B6", Turn.Black, GameState.Playing,
                "H1", "H3", "A5;A3");

            var result = executor.IsNewMoveValid();

            Assert.AreEqual(true, result);
        }

        [TestMethod]
        public void WhitePlayer_CannotMoveBlackPieces()
        {
            string board =
                "EEEEEEEK" +
                "EEEEEEEE" +
                "EEEEEEEk" +
                "EEEEEEEE" +
                "EEEEEEEE" +
                "EEEEEEEE" +
                "pEEEEEEE" +
                "EEEEEEEE";
            var executor = GetMoveExecutor(board, "A7;A6", Turn.White, GameState.Playing,
                "H1", "H3", "A5;A3");

            var result = executor.IsNewMoveValid();

            Assert.AreEqual(false, result);
        }

        [TestMethod]
        public void BlackPlayer_CannotMoveWhitePieces()
        {
            string board =
                "EEEEEEEK" +
                "PEEEEEEE" +
                "EEEEEEEk" +
                "EEEEEEEE" +
                "EEEEEEEE" +
                "EEEEEEEE" +
                "EEEEEEEE" +
                "EEEEEEEE";
            var executor = GetMoveExecutor(board, "A2;A3", Turn.Black, GameState.Playing,
                "H1", "H3", "A5;A3");

            var result = executor.IsNewMoveValid();

            Assert.AreEqual(false, result);
        }

        [TestMethod]
        public void Player_CannotTakeHisOwnPiece()
        {
            string board =
                "EEEEEEEK" +
                "PEEEEEEE" +
                "EPEEEEEk" +
                "EEEEEEEE" +
                "EEEEEEEE" +
                "EEEEEEEE" +
                "pEEEEEEE" +
                "EEEEEEEE";
            var executor = GetMoveExecutor(board, "A2;B3", Turn.White, GameState.Playing,
                "H1", "H3", "A5;A3");

            var result = executor.IsNewMoveValid();

            Assert.AreEqual(false, result);
        }

        [TestMethod]
        public void BlackPawn_CanTakeWithEnPassantRule()
        {
            string board =
                "EEEEEEEK" +
                "EEEEEEEE" +
                "EEEEEEEk" +
                "pPEEEEEE" +
                "EEEEEEEE" +
                "EEEEEEEE" +
                "EEEEEEEE" +
                "EEEEEEEE";
            var executor = GetMoveExecutor(board, "A4;B3", Turn.Black, GameState.Playing,
                "H1", "H3", "B2;B4");

            var result = executor.IsNewMoveValid();

            Assert.AreEqual(true, result);
        }

        [TestMethod]
        public void BlackPawn_CannotTakeWithEnPassantRuleIfLastMoveIsNotDoubleEnemyPawnMove()
        {
            string board =
                "EEEEEEEK" +
                "EEEEEEEE" +
                "EEEEEEEk" +
                "pPEEEEEE" +
                "EEEEEEEE" +
                "EEEEEEEE" +
                "EEEEEEEE" +
                "EEEEEEEE";
            var executor = GetMoveExecutor(board, "A4;B3", Turn.Black, GameState.Playing,
                "H1", "H3", "B3;B4");

            var result = executor.IsNewMoveValid();

            Assert.AreEqual(false, result);
        }

        [TestMethod]
        public void Rook_CanMoveHorizontally()
        {
            string board =
                "REEEEEEK" +
                "EEEEEEEE" +
                "EEEEEEEk" +
                "EEEEEEEE" +
                "EEEEEEEE" +
                "EEEEEEEE" +
                "EEEEEEEE" +
                "EEEEEEEE";
            var executor = GetMoveExecutor(board, "A1;G1", Turn.White, GameState.Playing,
                "H1", "H3", "A1;A2");

            var result = executor.IsNewMoveValid();

            Assert.AreEqual(true, result);
        }

        [TestMethod]
        public void Rook_CanMoveVertically()
        {
            string board =
                "REEEEEEK" +
                "EEEEEEEE" +
                "EEEEEEEk" +
                "EEEEEEEE" +
                "EEEEEEEE" +
                "EEEEEEEE" +
                "EEEEEEEE" +
                "EEEEEEEE";
            var executor = GetMoveExecutor(board, "A1;A8", Turn.White, GameState.Playing,
                "H1", "H3", "A1;A2");

            var result = executor.IsNewMoveValid();

            Assert.AreEqual(true, result);
        }

        [TestMethod]
        public void Rook_CannotJumpOverAPieceHorizontally()
        {
            string board =
                "REPEEEEK" +
                "EEEEEEEE" +
                "EEEEEEEk" +
                "EEEEEEEE" +
                "EEEEEEEE" +
                "EEEEEEEE" +
                "EEEEEEEE" +
                "EEEEEEEE";
            var executor = GetMoveExecutor(board, "A1;G1", Turn.White, GameState.Playing,
                "H1", "H3", "A1;A2");

            var result = executor.IsNewMoveValid();

            Assert.AreEqual(false, result);
        }

        [TestMethod]
        public void Rook_CannotJumpOverAPieceVertically()
        {
            string board =
                "REPEEEEK" +
                "EEEEEEEE" +
                "EEEEEEEk" +
                "EEEEEEEE" +
                "EEEEEEEE" +
                "EEEEEEEE" +
                "PEEEEEEE" +
                "EEEEEEEE";
            var executor = GetMoveExecutor(board, "A1;A8", Turn.White, GameState.Playing,
                "H1", "H3", "A1;A2");

            var result = executor.IsNewMoveValid();

            Assert.AreEqual(false, result);
        }

        [TestMethod]
        public void Rook_CanTakeAPiece()
        {
            string board =
                "REPEEEEK" +
                "EEEEEEEE" +
                "EEEEEEEk" +
                "EEEEEEEE" +
                "EEEEEEEE" +
                "EEEEEEEE" +
                "EEEEEEEE" +
                "rEEEEEEE";
            var executor = GetMoveExecutor(board, "A1;A8", Turn.White, GameState.Playing,
                "H1", "H3", "A1;A2");

            var result = executor.IsNewMoveValid();

            Assert.AreEqual(true, result);
        }

        [TestMethod]
        public void Bishop_CanMoveLongDiagonal()
        {
            string board =
                "BEEEEEEK" +
                "EEEEEEEE" +
                "EEEEEEEk" +
                "EEEEEEEE" +
                "EEEEEEEE" +
                "EEEEEEEE" +
                "EEEEEEEE" +
                "EEEEEEEE";
            var executor = GetMoveExecutor(board, "A1;H8", Turn.White, GameState.Playing,
                "H1", "H3", "A2;A3");

            var result = executor.IsNewMoveValid();

            Assert.AreEqual(true, result);
        }

        [TestMethod]
        public void Bishop_CanMoveShortDiagonal()
        {
            string board =
                "BEEEEEEK" +
                "EEEEEEEE" +
                "EEEEEEEk" +
                "EEEEEEEE" +
                "EEEEEEEE" +
                "EEEEEEEE" +
                "EEEEEEEE" +
                "EEEEEEEE";
            var executor = GetMoveExecutor(board, "A1;B2", Turn.White, GameState.Playing,
                "H1", "H3", "A2;A3");

            var result = executor.IsNewMoveValid();

            Assert.AreEqual(true, result);
        }

        [TestMethod]
        public void Bishop_CannotJumpOverAPiece()
        {
            string board =
                "BEEEEEEK" +
                "EPEEEEEE" +
                "EEEEEEEk" +
                "EEEEEEEE" +
                "EEEEEEEE" +
                "EEEEEEEE" +
                "EEEEEEEE" +
                "EEEEEEEE";
            var executor = GetMoveExecutor(board, "A1;H8", Turn.White, GameState.Playing,
                "H1", "H3", "A2;A3");

            var result = executor.IsNewMoveValid();

            Assert.AreEqual(false, result);
        }

        [TestMethod]
        public void Bishop_CanTakeAPiece()
        {
            string board =
                "BEEEEEEK" +
                "EpEEEEEE" +
                "EEEEEEEk" +
                "EEEEEEEE" +
                "EEEEEEEE" +
                "EEEEEEEE" +
                "EEEEEEEE" +
                "EEEEEEEE";
            var executor = GetMoveExecutor(board, "A1;B2", Turn.White, GameState.Playing,
                "H1", "H3");

            var result = executor.IsNewMoveValid();

            Assert.AreEqual(true, result);
        }

        [TestMethod]
        public void CanMakeQueenSideCastle()
        {
            string board =
                "REEEKEEE" +
                "EEEEEEEE" +
                "EEEEEEEk" +
                "EEEEEEEE" +
                "EEEEEEEE" +
                "EEEEEEEE" +
                "EEEEEEEE" +
                "EEEEEEEE";
            var executor = GetMoveExecutor(board, "E1;C1", Turn.White, GameState.Playing,
                "E1", "H3", "A2;A3");

            var result = executor.IsNewMoveValid();
            var newBoard = executor.NewBoardModelState.Board;
            var kingIsAtPlace = newBoard[0, 2] == BoardConstants.WHITE_KING;
            var rookIsAtPlace = newBoard[0, 3] == BoardConstants.WHITE_ROOK;
            var prevPositionsAreEmpty = newBoard[0, 0] == BoardConstants.EMPTY_FIELD && newBoard[0, 4] == BoardConstants.EMPTY_FIELD;

            Assert.AreEqual(true, result);
            Assert.AreEqual(true, kingIsAtPlace);
            Assert.AreEqual(true, rookIsAtPlace);
            Assert.AreEqual(true, prevPositionsAreEmpty);
        }

        [TestMethod]
        public void CanMakeKingSideCastle()
        {
            string board =
                "REEEKEER" +
                "EEEEEEEP" +
                "EEEEEEEk" +
                "EEEEEEEE" +
                "EEEEEEEE" +
                "EEEEEEEE" +
                "EEEEEEEE" +
                "EEEEEEEE";
            var executor = GetMoveExecutor(board, "E1;G1", Turn.White, GameState.Playing,
                "E1", "H3", "A2;A3");

            var result = executor.IsNewMoveValid();
            var newBoard = executor.NewBoardModelState.Board;
            var kingIsAtPlace = newBoard[0, 6] == BoardConstants.WHITE_KING;
            var rookIsAtPlace = newBoard[0, 5] == BoardConstants.WHITE_ROOK;
            var prevPositionsAreEmpty = newBoard[0, 7] == BoardConstants.EMPTY_FIELD && newBoard[0, 4] == BoardConstants.EMPTY_FIELD;

            Assert.AreEqual(true, result);
            Assert.AreEqual(true, kingIsAtPlace);
            Assert.AreEqual(true, rookIsAtPlace);
            Assert.AreEqual(true, prevPositionsAreEmpty);
        }

        [TestMethod]
        public void CannotMakeKingSideCastleIfKingIsCheck()
        {
            string board =
                "REEEKEER" +
                "EEEEEEEP" +
                "EEEEEEbk" +
                "EEEEEEEE" +
                "EEEEEEEE" +
                "EEEEEEEE" +
                "EEEEEEEE" +
                "EEEEEEEE";
            var executor = GetMoveExecutor(board, "E1;G1", Turn.White, GameState.Playing,
                "E1", "H3", "A2;A3");

            var result = executor.IsNewMoveValid();

            Assert.AreEqual(false, result);
        }

        [TestMethod]
        public void CannotMakeQueenSideCastleIfKingIsCheck()
        {
            string board =
                "REEEKEER" +
                "EEEEEEEP" +
                "EEEEEEbk" +
                "EEEEEEEE" +
                "EEEEEEEE" +
                "EEEEEEEE" +
                "EEEEEEEE" +
                "EEEEEEEE";
            var executor = GetMoveExecutor(board, "E1;C1", Turn.White, GameState.Playing,
                "E1", "H3", "A2;A3");

            var result = executor.IsNewMoveValid();

            Assert.AreEqual(false, result);
        }
    }
}
