using MultiplayerWebChess.Domain.Migrations;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MultiplayerWebChess.Domain.Entities;

namespace MultiplayerWebChess.Domain
{
    public class ChessContext : DbContext
    {
        public ChessContext()
            : base("DefaultConnection") { }

        public ChessContext(string connectionString)
            : base(connectionString) { }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            Database.SetInitializer(new MigrateDatabaseToLatestVersion<ChessContext, Configuration>());
        }

        public DbSet<UserProfile> UserProfiles { get; set; }
        public DbSet<Game> Games { get; set; }
        public DbSet<BoardMove> BoardMoves { get; set; }
    }
}
