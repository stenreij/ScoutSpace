using Core.Domain;

namespace Core.DomainServices
{
    public interface IScoutRepo
    {
        IEnumerable<Scout> GetScouts();
    }
}