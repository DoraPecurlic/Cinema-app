namespace Cinema.Model;

public class GetPayment
{
    public Guid Id { get; set; }
    public decimal TotalPrice { get; set; }
    public DateTime PaymentDate { get; set; }
}