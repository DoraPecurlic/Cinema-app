using Cinema.Model;

namespace Cinema.Service.Common
{
    public interface IActorService
    {
        Task AddActorAsync(Actor actor);
        Task<IEnumerable<Actor>> GetAllActorsAsync();
        Task<Actor> GetActorByIdAsync(Guid id);
        Task UpdateActorAsync(Actor actor);
        Task DeleteActorAsync(Guid id);
    }
}
