using Cinema.Model;
using Cinema.Repository.Common;
using Cinema.Service.Common;
using System;
using System.Threading.Tasks;

namespace Cinema.Service
{
    public class PaymentService : IPaymentService
    {
        private readonly IPaymentRepository _paymentRepository;

        public PaymentService(IPaymentRepository paymentRepository)
        {
            _paymentRepository = paymentRepository;
        }

        public async Task<Guid>  AddPaymentAsync(Payment payment)
        { 
            await _paymentRepository.AddPaymentAsync(payment);

            return payment.Id; 
        }
        
        public async Task<List<GetPayment>> GetAllPaymentsAsync()
        {
            return await _paymentRepository.GetAllPaymentsAsync();
        }

        public async Task<List<GetPayment>> GetPaymentsByUserAsync(Guid userId)
        {
            return await _paymentRepository.GetPaymentsByUserAsync(userId);
        }

    }
}
