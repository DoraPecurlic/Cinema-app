using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cinema.Model
{
    public class Seat
    {
        public Guid Id { get; set; }
        public int SeatNumber { get; set; }
        public string RowLetter { get; set; }
        public Guid HallId { get; set; }
        public int HallNumber { get; set; } 
    }
}

