using Microsoft.AspNet.SignalR;
using MultiplayerWebChess.ChessLogic;
using MultiplayerWebChess.Domain.DomainContext;
using MultiplayerWebChess.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebMatrix.WebData;

namespace MultiplayerWebChess.Hubs
{
    public class Chess : Hub
    {
        private IDatabase db;

        public Chess(IDatabase database)
        {
            db = database;
        }

        public void PlayMove(string gameId, string move)
        {
            Guid gId;
            if (Guid.TryParse(gameId, out gId))
            {
                Game game = db.Games.Get()
                    .IncludeMultiple(x => x.BoardMoves)
                    .FirstOrDefault(x => x.Id == gId && x.GameState == GameState.Playing);

                if (game != null)
                {
                    if (IsPlayerTurn(game, WebSecurity.CurrentUserId))
                    {
                        string lastMove = (game.BoardMoves.Count == 0) ? null
                            : game.BoardMoves.OrderByDescending(x => x.DateTimePlayed).First().MoveContent;

                        IMoveExecutor executor = new DefaultMoveExecutor(
                            game.BoardContent, 
                            move,
                            game.Turn, game.GameState, 
                            game.WhiteKingPosition,
                            game.BlackKingPosition, 
                            lastMove, 
                            game.WhiteCanCastleKingSide,
                            game.WhiteCanCastleQueenSide, 
                            game.BlackCanCastleKingSide,
                            game.BlackCanCastleQueenSide);

                        if (executor.IsNewMoveValid())
                        {
                            BoardModel model = executor.NewBoardModelState;
                            game.BoardContent = model.GetBoardAsString();
                            game.Turn = model.Turn;
                            game.GameState = model.GameState;
                            game.WhiteKingPosition = model.WhiteKingPosition.ToString();
                            game.BlackKingPosition = model.BlackKingPosition.ToString();
                            game.WhiteCanCastleKingSide = model.WhiteCanCastleKingSide;
                            game.WhiteCanCastleQueenSide = model.WhiteCanCastleQueenSide;
                            game.BlackCanCastleKingSide = model.BlackCanCastleKingSide;
                            game.BlackCanCastleQueenSide = model.BlackCanCastleQueenSide;

                            BoardMove newMove = new BoardMove
                            {
                                MoveContent = move,
                                DateTimePlayed = DateTime.Now
                            };

                            game.BoardMoves.Add(newMove);
                            db.Games.Update(game);
                            db.Save();

                            Clients.Group(gameId).updateViewModel(game.BoardContent);
                        }
                        else
                        {
                            Clients.Caller.updateViewModel(game.BoardContent);
                        }
                    }
                }
            }
        }

        public void GetBoard(string gameId)
        {
            Guid gId;
            Game game;
            if (Guid.TryParse(gameId, out gId) &&
                ((game = db.Games.GetById(gId)) != null))
            {
                Clients.Caller.updateViewModel(game.BoardContent);
            }
        }

        public void SendGameId(string gameId)
        {
            Guid gId;
            if (Guid.TryParse(gameId, out gId) &&
                (db.Games.GetById(gId) != null))
            {
                InitConnection(gameId);
            }
        }

        public void SendChatMessage(string gameId, string message)
        {
            Guid gId;
            Game game;
            if (Guid.TryParse(gameId, out gId) &&
                ((game = db.Games.GetById(gId)) != null))
            {
                Clients.Group(game.Id.ToString()).addChatMessage(HttpUtility.HtmlEncode(message));
            }
        }

        // Non-client access
        private void InitConnection(string gameId)
        {
            Groups.Add(Context.ConnectionId, gameId);
            Clients.OthersInGroup(gameId).addChatMessage(
                HttpUtility.HtmlEncode(WebSecurity.CurrentUserName + " connected."));
        }

        private bool IsPlayerTurn(Game game, int userId)
        {
            if ((game.Turn == Turn.White && userId == game.WhitePlayerId) ||
                (game.Turn == Turn.Black && userId == game.BlackPlayerId))
            {
                return true;
            }

            return false;
        }
    }
}