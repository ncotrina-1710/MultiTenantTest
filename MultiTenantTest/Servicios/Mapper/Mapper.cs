using AutoMapper;
using MultiTenantTest.Servicios.Organization.Dtos;
using MultiTenantTest.Servicios.Product.Dtos;
using MultiTenantTest.Servicios.Vinculation.Dtos;

namespace MultiTenantTest.Servicios.Mapper
{
    public class Mapper: Profile
    {
        public Mapper()
        {
            CreateMap<Dominio.Entidades.Vinculation, VinculationDto>().ReverseMap();
            CreateMap<Dominio.Entidades.Organization, OrganizationDto>().ReverseMap();
            CreateMap<Dominio.Entidades.Product, ProductDto>().ReverseMap();
        }

    }
}
