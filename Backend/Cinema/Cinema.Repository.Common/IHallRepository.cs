using Cinema.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DTO.HallModel;

namespace Cinema.Repository.Common
{
    public interface IHallRepository
    {
        Task AddHallAsync(Hall hall);
        Task<IEnumerable<Hall>> GetAllHallsAsync();
        Task<Hall?> GetHallByIdAsync(Guid id);
        Task<List<AvailableHallGet>> GetAvailableHallsAsync(DateOnly date, TimeOnly startTime, TimeOnly endTime);
        Task<Hall?> GetHallByProjectionIdAsync(Guid projectionId); 
        Task UpdateHallAsync(Hall hall);
        Task DeleteHallAsync(Guid id);
    }
}
