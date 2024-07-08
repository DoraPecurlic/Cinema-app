namespace Cinema.Model;

public class CreatePayment
{
    public decimal TotalPrice { get; set; }
    public Guid UserId { get; set; }
}