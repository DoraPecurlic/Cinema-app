using AutoMapper;
using Cinema.Model;
using DTO.TicketModel;

namespace Cinema.Mapper
{
    public class TicketProfile : Profile
    {
        public TicketProfile()
        {
            CreateMap<Ticket, TicketRest>().ReverseMap();
        }
    }
}
