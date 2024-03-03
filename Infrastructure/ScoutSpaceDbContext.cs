using Core.Domain;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure
{
    public class ScoutSpaceDbContext : DbContext
    {
        public ScoutSpaceDbContext(DbContextOptions<ScoutSpaceDbContext> options) : base(options) { }

        public DbSet<Scout> Scout { get; set; }
        public DbSet<Player> Player { get; set; }
        public DbSet<Team> Team { get; set; }
        public DbSet<Note> Note { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            modelBuilder.Entity<Player>()
                .HasMany(p => p.notities)
                .WithOne()
                .OnDelete(DeleteBehavior.Cascade);

        }
    }
}