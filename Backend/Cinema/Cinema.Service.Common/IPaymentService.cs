using Cinema.Model;
using System.Threading.Tasks;

namespace Cinema.Service.Common
{
    public interface IPaymentService
    {
        Task<Guid> AddPaymentAsync(Payment payment);
        Task<List<GetPayment>> GetAllPaymentsAsync();
        Task<List<GetPayment>> GetPaymentsByUserAsync(Guid userId);
    }
}
