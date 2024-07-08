namespace Cinema.Common
{
    public class MovieFiltering
    {
        public Guid? MovieId { get; set; }
        public Guid? GenreId { get; set; }
        public Guid? LanguageId { get; set; }
        public string SearchTerm { get; set; } 
    }
}