using Cinema.Model;
using System.Threading.Tasks;

namespace Cinema.Repository.Common
{
    public interface IPaymentRepository
    {
        Task AddPaymentAsync(Payment payment);
        Task<List<GetPayment>> GetAllPaymentsAsync();
        Task<List<GetPayment>> GetPaymentsByUserAsync(Guid userId);
    }
}
