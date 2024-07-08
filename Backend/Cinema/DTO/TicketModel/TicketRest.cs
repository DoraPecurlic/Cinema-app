namespace DTO.TicketModel
{
    public class TicketRest
    {
        public Guid Id { get; set; }
        public decimal Price { get; set; }
        public Guid PaymentId { get; set; }
        public Guid UserId { get; set; }
        public Guid ProjectionId { get; set; }
        public bool IsActive { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime DateUpdated { get; set; }
        public Guid CreatedByUserId { get; set; }
        public Guid UpdatedByUserId { get; set; }
    }
}
