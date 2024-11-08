using AutoMapper;
using FlightAPI.Data.DTO;
using FlightAPI.Models;


namespace FlightAPI.Mapping
{
    public class FlightMapping : Profile
    {
        public FlightMapping()
        {
            CreateMap<Flight, FlightDTO>()
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status.ToString()));

            CreateMap<FlightDTO, Flight>()
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => Enum.Parse<FlightStatus>(src.Status)));
        }
    }
}

