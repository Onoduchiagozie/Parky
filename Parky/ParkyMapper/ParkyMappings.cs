using AutoMapper;
using Parky.Models;
using Parky.Models.DTO;

namespace Parky.ParkyMapper;

public class ParkyMappings:Profile
{
    public ParkyMappings()
    {
        CreateMap<NationPark, NationParkDto>().ReverseMap();
        CreateMap<Trail, TrailDto>().ReverseMap();
        CreateMap<Trail, CreateDto>().ReverseMap();
        CreateMap<Trail, UpdateDto>().ReverseMap();
    }
}