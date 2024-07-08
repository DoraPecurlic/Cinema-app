namespace DTO.ProjectionModel
{
    public class GetProjectionRest
    {
        public Guid Id { get; set; }
        public DateTime Date { get; set; }
        public TimeSpan Time { get; set; }
        public Guid MovieId { get; set; }
        public string? Title { get; set; }
        public Guid UserId { get; set; }
        public DateTime DateCreated { get; set; }
    }
}
