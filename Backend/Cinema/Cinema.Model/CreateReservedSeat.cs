namespace Cinema.Model
{
    public class CreateReservedSeat
    {
        public Guid TicketId { get; set; }
        public Guid ProjectionId { get; set; }
        public Guid SeatId { get; set; }
        public Guid UserId { get; set; }
    }
}