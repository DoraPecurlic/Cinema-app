using Cinema.Model;
using DTO.HallModel;

namespace Cinema.Service.Common
{
    public interface IHallService
    {
        Task AddHallAsync(Hall hall);
        Task<IEnumerable<Hall>> GetAllHallsAsync();
        Task<Hall> GetHallByIdAsync(Guid id);
        Task<List<AvailableHallGet>> GetAvailableHallsAsync(DateOnly date, TimeOnly time, Guid movieId);
        Task<Hall?> GetHallByProjectionIdAsync(Guid projectionId);
        Task UpdateHallAsync(Hall hall);
        Task DeleteHallAsync(Guid id);
    }
}
