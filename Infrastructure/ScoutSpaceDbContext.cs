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
        public DbSet<Notitie> Notitie { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Voeg hier je andere configuraties toe

            // Configureer de relatie tussen Player en Notitie
            modelBuilder.Entity<Player>()
                .HasMany(p => p.notities)
                .WithOne()
                .OnDelete(DeleteBehavior.Cascade);

        }
    }
}