namespace Cinema.Model;

public class SeatDetails
{
    public Guid Id { get; set; }
    public string RowLetter { get; set; }
    public int SeatNumber { get; set; }
    public int HallNumber { get; set; }
}
public class EmailRequest
{ 
    public string UserEmail { get; set; }
    public List<Guid> TicketIds { get; set; }
    public List<SeatDetails> Seats { get; set; }
    
}
