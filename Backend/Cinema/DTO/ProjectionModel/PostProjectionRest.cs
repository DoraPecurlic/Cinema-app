namespace DTO.ProjectionModel
{
    public class PostProjectionRest
    {
        public DateTime Date { get; set; }
        public TimeSpan Time { get; set; }
        public Guid MovieId { get; set; }
        public Guid UserId { get; set; }
        public bool IsActive { get; set; }
        public Guid CreatedByUserId { get; set; }
        public Guid UpdatedByUserId { get; set; }
        public List<PostProjectionHallRest> ProjectionHalls { get; set; } = new List<PostProjectionHallRest>();
    }

    public class PostProjectionHallRest
    {
        public Guid HallId { get; set; }
    }
}
