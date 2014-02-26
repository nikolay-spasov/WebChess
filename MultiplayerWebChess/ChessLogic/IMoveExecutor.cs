using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MultiplayerWebChess.ChessLogic
{
    public interface IMoveExecutor
    {
        bool IsNewMoveValid();

        BoardModel NewBoardModelState { get; }
    }
}