using Cinema.Model;

namespace Cinema.Repository.Common
{
    public interface IActorRepository
    {
        Task AddActorAsync(Actor actor);
        Task<IEnumerable<Actor>> GetAllActorsAsync();
        Task<Actor?> GetActorAsync(Guid id);
        Task<List<Actor>> GetActorsByNameAsync(List<string> actorNames);
        Task<Actor?> GetActorByNameAsync(string actorName);
        Task UpdateActorAsync(Actor actor);
        Task DeleteActorAsync(Guid id);
    }
}
