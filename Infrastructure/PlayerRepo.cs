using Core.Domain;
using Core.DomainServices;
using Core.DomainServices.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure
{
    public class PlayerRepo : IPlayerRepo
    {
        private readonly ScoutSpaceDbContext _context;

        public PlayerRepo(ScoutSpaceDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Player>> GetAllPlayersAsync()
        {
            return await _context.Player
                .Include(p => p.team)
                .Include(p => p.notities)
                .ToListAsync();
        }

        public async Task<Player> GetPlayerByIdAsync(int id)
        {
            return await _context.Player
                .Include(p => p.team)
                .Include(p => p.notities)
                .FirstAsync(p => p.playerId == id);
        }

        public async Task UpdatePlayerAsync(Player player)
        {
            var playerToUpdate = await _context.Player.FindAsync(player.playerId);

            if (playerToUpdate != null)
            {
                _context.Entry(playerToUpdate).CurrentValues.SetValues(player);
                await _context.SaveChangesAsync();
            }
        }

        public async Task DeletePlayerAsync(int id)
        {
            var playerToDelete = await _context.Player.FindAsync(id);

            if (playerToDelete != null)
            {
                _context.Remove(playerToDelete);
                await _context.SaveChangesAsync();
            }
        }
    }
}