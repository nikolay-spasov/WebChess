using MultiplayerWebChess.Domain.DomainContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MultiplayerWebChess.Domain.Entities;
using WebMatrix.WebData;
using MultiplayerWebChess.Models;
using MultiplayerWebChess.ChessLogic;
using Microsoft.AspNet.SignalR;

namespace MultiplayerWebChess.Controllers
{
    public class GameController : Controller
    {
        private static Random rand = new Random();
        private static readonly IHubContext gameLobbyHubContext =
            GlobalHost.ConnectionManager.GetHubContext<Hubs.GameLobby>();

        private IDatabase db;

        public GameController(IDatabase database)
        {
            db = database;
        }

        public ActionResult Game(string id)
        {
            Guid gameId = Guid.Parse(id);

            Game game = db.Games.Get().FirstOrDefault(x => x.Id == gameId);

            GameVM viewModel = new GameVM
            {
                GameId = game.Id.ToString(),
                Flipped = WebSecurity.CurrentUserId == game.BlackPlayerId,
                IsWhite = WebSecurity.CurrentUserId == game.WhitePlayerId
            };

            return View(viewModel);
        }

        public ActionResult GameLobby()
        {
            CreateGameVM viewModel = new CreateGameVM
            {
                ColorSelect = ColorSelect.Auto
            };
            return View(viewModel);
        }

        [System.Web.Mvc.Authorize]
        [HttpPost]
        public ActionResult CreateGame(CreateGameVM game)
        {
            if (ModelState.IsValid)
            {
                Game newGame = new Game();
                newGame.Id = Guid.NewGuid();
                newGame.HostId = WebSecurity.CurrentUserId;
                newGame.GameState = GameState.Waiting;
                newGame.DateCreated = DateTime.Now;
                newGame.Turn = Turn.White;
                newGame.Description = game.Description;
                newGame.WhiteCanCastleKingSide = true;
                newGame.WhiteCanCastleQueenSide = true;
                newGame.BlackCanCastleKingSide = true;
                newGame.BlackCanCastleQueenSide = true;
                newGame.WhiteKingPosition = BoardConstants.INITIAL_WHITE_KING_POSITION;
                newGame.BlackKingPosition = BoardConstants.INITIAL_BLACK_KING_POSITION;
                newGame.BoardContent = BoardConstants.INITIAL_BOARD;
                if (game.ColorSelect == ColorSelect.White)
                {
                    newGame.WhitePlayerId = WebSecurity.CurrentUserId;
                }
                else if (game.ColorSelect == ColorSelect.Black)
                {
                    newGame.BlackPlayerId = WebSecurity.CurrentUserId;
                }
                else
                {
                    int r = rand.Next(0, 2);
                    if (r == 0)
                    {
                        newGame.WhitePlayerId = WebSecurity.CurrentUserId;
                    }
                    else
                    {
                        newGame.BlackPlayerId = WebSecurity.CurrentUserId;
                    }
                }

                db.Games.Insert(newGame);
                db.Save();

                gameLobbyHubContext.Clients.All.addGame(newGame.Id.ToString(), newGame.HostId,
                    newGame.Description);

                return RedirectToAction("Game", new { id = newGame.Id });
            }
            else
            {
                return View("GameLobby");
            }
        }

        public ActionResult JoinGame(string id)
        {
            Guid gId = Guid.Parse(id);

            Game gameToJoin = db.Games.Get().FirstOrDefault(x => x.Id == gId);
            if (gameToJoin.WhitePlayerId == null)
            {
                gameToJoin.WhitePlayerId = WebSecurity.CurrentUserId;
            }
            else
            {
                gameToJoin.BlackPlayerId = WebSecurity.CurrentUserId;
            }
            gameToJoin.GameState = GameState.Playing;

            db.Games.Update(gameToJoin);
            db.Save();

            gameLobbyHubContext.Clients.All.removeGame(id.ToString());

            return RedirectToAction("Game", new { id = gId });
        }
    }
}
