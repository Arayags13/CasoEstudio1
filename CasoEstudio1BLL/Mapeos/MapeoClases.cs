using AutoMapper;
using CasoEstudio1BLL.Dtos;
using CasoEstudio1DAL.Entidades;
using System.Linq;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace CasoEstudio1BLL.Mapeos
{
    public class MapeoClases : Profile
    {
        public MapeoClases()
        {
            // Cliente
            CreateMap<Cliente, ClienteDto>().ReverseMap();

            // Vehículo
            CreateMap<Vehiculo, VehiculoDto>().ReverseMap();

            // Cita
            // Mapeo de Entidad a DTO para mostrar detalles en la lista
            CreateMap<Cita, CitaDto>()
                .ForMember(dest => dest.ClienteNombreCompleto, opt => opt.Ignore()) // Se llenará en el servicio
                .ForMember(dest => dest.VehiculoPlaca, opt => opt.Ignore()); // Se llenará en el servicio

            // Mapeo de DTO a Entidad
            CreateMap<CitaDto, Cita>();
        }
    }
}