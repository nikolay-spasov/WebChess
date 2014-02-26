using MultiplayerWebChess.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MultiplayerWebChess.Domain.DomainContext
{
    public class DatabaseContext : IDatabase
    {
        private ChessContext context = new ChessContext();
        private IGenericRepository<UserProfile> userProfileRepository;
        private IGenericRepository<Game> gameRepository;

        private bool disposed = false;

        public IGenericRepository<UserProfile> UserProfiles
        {
            get
            {
                if (this.userProfileRepository == null)
                {
                    this.userProfileRepository = new GenericRepository<UserProfile>(context);
                }
                return this.userProfileRepository;
            }
        }

        public IGenericRepository<Game> Games
        {
            get
            {
                if (this.gameRepository == null)
                {
                    this.gameRepository = new GenericRepository<Game>(context);
                }
                return this.gameRepository;
            }
        }

        public void Save()
        {
            context.SaveChanges();
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    context.Dispose();
                }
            }
            this.disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
