namespace Cinema.Model
{
    public class CreateTicket
    {
        public decimal Price { get; set; }
        public Guid PaymentId { get; set; }
        public Guid ProjectionId { get; set; }
        public Guid UserId { get; set; } 
    }
}