using Cinema.Model;
using Cinema.Repository.Common;
using Cinema.Service.Common;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Cinema.Service
{
    public class TicketService : ITicketService
    {
        private readonly ITicketRepository _ticketRepository;
        private readonly IPaymentService _paymentService;

        public TicketService(ITicketRepository ticketRepository, IPaymentService paymentService)
        {
            _ticketRepository = ticketRepository;
            _paymentService = paymentService;
        }
        
        public async Task<Guid>  AddTicketAsync(Ticket ticket)
        {
            await _ticketRepository.AddTicketAsync(ticket);
            return ticket.Id;
        }

       
    }
}
