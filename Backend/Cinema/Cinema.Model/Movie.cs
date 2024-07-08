using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cinema.Model
{
    public class Movie
    {
        public Guid Id { get; set; }
        public string? Title { get; set; }
        public Guid GenreId { get; set; }
        public string? Description { get; set; }
        public int Duration { get; set; }
        public Guid LanguageId { get; set; }
        public string? CoverUrl { get; set; }
        public string? TrailerUrl { get; set; }
        public bool IsActive { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime DateUpdated { get; set; }
        public Guid CreatedByUserId { get; set; }
        public Guid UpdatedByUserId { get; set; }
        public List<Guid>? ActorId { get; set; }

    }
}
