using Core.Domain;
using Core.DomainServices.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DomainServices.Services
{
    public class PlayerService : IPlayerService
    {
        private readonly IPlayerRepo _playerRepo;

        public PlayerService(IPlayerRepo playerRepo)
        {
            _playerRepo = playerRepo;
        }

        Task<IEnumerable<Player>> IPlayerService.GetAllPlayersAsync()
        {
            return _playerRepo.GetAllPlayersAsync();
        }

        Task<Player> IPlayerService.GetPlayerByIdAsync(int id)
        { 
            return _playerRepo.GetPlayerByIdAsync(id); 
        }

        Task IPlayerService.UpdatePlayerAsync(Player player)
        {
            return _playerRepo.UpdatePlayerAsync(player);
        }

        Task IPlayerService.DeletePlayerAsync(int id)
        {
            return _playerRepo.DeletePlayerAsync(id);
        }
    }
}
