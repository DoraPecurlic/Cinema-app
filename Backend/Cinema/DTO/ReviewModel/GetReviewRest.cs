namespace DTO.ReviewModel
{
    public class GetReviewRest
    {
        public Guid Id { get; set; }
        public string Description { get; set; }
        public int Rating { get; set; }
        public Guid UserId { get; set; }
        public string FirstName { get; set; }
        public Guid MovieId { get; set; }
        public DateTime DateCreated { get; set; }
        public Guid UpdatedByUserId { get; set; }
    }
}
