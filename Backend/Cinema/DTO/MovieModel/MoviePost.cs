namespace DTO.MovieModel
{
    public class MoviePost
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public int Duration { get; set; }
        public Guid LanguageId { get; set; }
        public string CoverUrl { get; set; }
        public string TrailerUrl { get; set; }
        public Guid GenreId { get; set; }
        public List<Guid> ActorIds { get; set; }
        public Guid CreatedByUserId { get; set; }
        public Guid UpdatedByUserId { get; set; }
    }
}
