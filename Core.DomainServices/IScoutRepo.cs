using Core.Domain;

namespace Core.DomainServices
{
    public interface IScoutRepo
    {
        Task<IEnumerable<Scout>> GetAllScoutsAsync();
    }
}