using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cinema.Model
{
    public class Ticket
    {
        public Guid Id { get; set; }
        public decimal Price { get; set; }
        public Guid PaymentId { get; set; }
        public Guid UserId { get; set; }
        public Guid ProjectionId { get; set; }
        public bool IsActive { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime DateUpdated { get; set; }
        public Guid CreatedByUserId { get; set; }
        public Guid UpdatedByUserId { get; set; }
       
    }
}
