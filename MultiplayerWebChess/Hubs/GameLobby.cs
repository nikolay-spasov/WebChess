using Microsoft.AspNet.SignalR;
using MultiplayerWebChess.Domain.DomainContext;
using MultiplayerWebChess.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MultiplayerWebChess.Hubs
{
    public class GameLobby : Hub
    {
        private class GameVM
        {
            public Guid gameId { get; set; }
            public string host { get; set; }
            public string description { get; set; }
        }

        private IDatabase db;

        public GameLobby(IDatabase database)
        {
            db = database;
        }

        public void GetGames()
        {
            DateTime fiveMinutesAgo = DateTime.Now.AddMinutes(-5);

            List<GameVM> games = db.Games.Get()
                .IncludeMultiple(x => x.Host)
                .Where(x => x.GameState == GameState.Waiting && x.DateCreated > fiveMinutesAgo)
                .OrderByDescending(x => x.DateCreated)
                .Select(x => new GameVM
                {
                    gameId = x.Id,
                    description = x.Description,
                    host = x.Host.UserName
                }).ToList();

            Clients.Caller.updateGames(games);
        }
    }
}