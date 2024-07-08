using Cinema.Model;
using Cinema.Service.Common;
using Microsoft.AspNetCore.Mvc;

namespace Cinema.WebApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PaymentController : Controller
    {
        private readonly IPaymentService _paymentService;

        public PaymentController(IPaymentService paymentService)
        {
            _paymentService = paymentService;
        }
        [HttpPost]
        public async Task<IActionResult> AddPaymentAsync([FromBody] CreatePayment createPayment)
        {
            var payment = new Payment
            {
                Id = Guid.NewGuid(),
                TotalPrice = createPayment.TotalPrice,
                PaymentDate = DateTime.UtcNow,
                IsActive = true,
                DateCreated = DateTime.UtcNow,
                DateUpdated = DateTime.UtcNow,
                CreatedByUserId = createPayment.UserId,
                UpdatedByUserId = createPayment.UserId
            };
            
            var paymentId = await _paymentService.AddPaymentAsync(payment);
            return Ok(new { PaymentId = paymentId });
        }
        
        [HttpGet]
        public async Task<IActionResult> GetAllPaymentsAsync()
        {
            try
            {
                var payments = await _paymentService.GetAllPaymentsAsync();
                return Ok(payments);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Failed to retrieve payments: {ex.Message}");
            }
        }

        [HttpGet("{userId}")]
        public async Task<IActionResult> GetPaymentsByUserAsync(Guid userId)
        {
            try
            {
                var payments = await _paymentService.GetPaymentsByUserAsync(userId);
                return Ok(payments);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Failed to retrieve payments for user with ID {userId}: {ex.Message}");
            }
        }
        

    }
}