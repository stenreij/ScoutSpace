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

        async Task IPlayerService.UpdatePlayerAsync(Player player)
        {
            bool isNewPlayer = await _playerRepo.IsNotExistingPlayerUpdateAsync(player.firstName, player.lastName, player.birthDate, player.playerId);

            if (!isNewPlayer)
            {
                throw new InvalidOperationException($"Speler {player.firstName} {player.lastName} {player.birthDate} bestaat al.");
            }

            await _playerRepo.UpdatePlayerAsync(player);
        }

        Task IPlayerService.DeletePlayerAsync(int id)
        {
            return _playerRepo.DeletePlayerAsync(id);
        }

        async Task IPlayerService.AddPlayerAsync(Player player)
        {
            bool isNewPlayer = await _playerRepo.IsNotExistingPlayerAsync(player.firstName, player.lastName, player.birthDate);

            if (!isNewPlayer) 
            {
                throw new InvalidOperationException($"Speler {player.firstName} {player.lastName} {player.birthDate} bestaat al.");
            }
            await _playerRepo.AddPlayerAsync(player);
        }

        public Task<bool> IsNewPlayerAsync(string firstName, string lastName, DateTime birthDate) 
        { 
            return _playerRepo.IsNotExistingPlayerAsync(firstName, lastName, birthDate);
        }

        public Task<bool> IsNotExistingPlayerUpdateAsync(string firstName, string lastName, DateTime birthDate, int playerId)
        {
            return _playerRepo.IsNotExistingPlayerUpdateAsync(firstName, lastName, birthDate, playerId);
        }
    }
}
