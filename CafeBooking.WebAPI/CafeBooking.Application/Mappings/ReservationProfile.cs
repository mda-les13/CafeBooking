using AutoMapper;
using CafeBooking.DataAccess.Entities;
using CafeBooking.BusinessLogic.DTOs;

namespace CafeBooking.BusinessLogic.Mappings
{
    public class ReservationProfile : Profile
    {
        public ReservationProfile()
        {
            CreateMap<ReservationCreateDto, Reservation>()
                .ForMember(dest => dest.EndDateTime, opt =>
                    opt.Ignore());

            CreateMap<Reservation, ReservationDto>()
                .ForMember(dest => dest.EndDateTime, opt =>
                    opt.MapFrom(src => src.EndDateTime));

            CreateMap<Table, TableDto>();
        }
    }
}
