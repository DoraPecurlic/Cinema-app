namespace DTO.ProjectionModel
{
    public class PutProjectionRest
    {
        public DateTime Date { get; set; }
        public TimeSpan Time { get; set; }
        public Guid MovieId { get; set; }
        public Guid UserId { get; set; }
        public bool IsActive { get; set; }
        public Guid CreatedByUserId { get; set; }
        public Guid UpdatedByUserId { get; set; }
    }
}
