using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cinema.Model
{
    public class MovieActor
    {
        public Guid Id { get; set; }
        public Guid MovieId { get; set; }
        public Guid ActorId { get; set; }
        public Movie? Movie { get; set; }
        public Actor? Actor { get; set; }
    }
}
