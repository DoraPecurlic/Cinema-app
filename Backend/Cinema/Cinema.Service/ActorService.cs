using Cinema.Model;
using Cinema.Repository.Common;

namespace Cinema.Service.Common
{
    public class ActorService : IActorService
    {
        private readonly IActorRepository _actorRepository;

        public ActorService(IActorRepository actorRepository)
        {
            this._actorRepository = actorRepository;
        }

        public async Task AddActorAsync(Actor actor)
        {
            actor.Id = Guid.NewGuid();
            actor.DateCreated = DateTime.UtcNow;
            actor.DateUpdated = DateTime.UtcNow;
            actor.IsActive = true;

            await _actorRepository.AddActorAsync(actor);
        }

        public async Task<IEnumerable<Actor>> GetAllActorsAsync()
        {
            return await _actorRepository.GetAllActorsAsync();
        }

        public async Task<Actor> GetActorByIdAsync(Guid id)
        {
            return await _actorRepository.GetActorAsync(id);
        }

        public async Task UpdateActorAsync(Actor actor)
        {
            actor.DateUpdated = DateTime.UtcNow;
            await _actorRepository.UpdateActorAsync(actor);
        }

        public async Task DeleteActorAsync(Guid id)
        {
            await _actorRepository.DeleteActorAsync(id);
        }
    }
}
