using MultiplayerWebChess.Domain.Entities;
using System;

namespace MultiplayerWebChess.Domain.DomainContext
{
    public interface IDatabase : IDisposable
    {
        IGenericRepository<UserProfile> UserProfiles { get; }

        IGenericRepository<Game> Games { get; }

        void Save();
    }
}
