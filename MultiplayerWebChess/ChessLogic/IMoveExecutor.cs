namespace MultiplayerWebChess.ChessLogic
{
    public interface IMoveExecutor
    {
        bool IsNewMoveValid();

        BoardModel NewBoardModelState { get; }
    }
}