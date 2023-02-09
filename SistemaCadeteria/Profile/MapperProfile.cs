using AutoMapper;
using SistemaCadeteria.Models;
using SistemaCadeteria.ViewModels;

namespace SistemaCadeteria.Profiles
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Cadete, CadeteViewModel>().ReverseMap();
            CreateMap<Cadete, CrearCadeteViewModel>().ReverseMap();
            CreateMap<Cadete, EditarCadeteViewModel>().ReverseMap();

            CreateMap<Cliente, ClienteViewModel>().ReverseMap();
            CreateMap<Cliente, CrearClienteViewModel>().ReverseMap();
            CreateMap<Cliente, EditarClienteViewModel>().ReverseMap();

            CreateMap<Pedido, PedidoViewModel>().ReverseMap();
        }
    }
}