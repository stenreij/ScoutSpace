using Core.Domain;

namespace Core.DomainServices.Interfaces
{
    public interface IScoutRepo
    {
        Task<IEnumerable<Scout>> GetAllScoutsAsync();
    }
}