using AutoMapper;
using FondoXYZ.web.Models;
using FondoXYZ.web.Services.Dto;

namespace FondoXYZ.web.Services.Mapper;
public class AlojamientoMapper:Profile
{
    public AlojamientoMapper()
    {
        CreateMap<Alojamiento, AlojamientoDto>();
    }
}
