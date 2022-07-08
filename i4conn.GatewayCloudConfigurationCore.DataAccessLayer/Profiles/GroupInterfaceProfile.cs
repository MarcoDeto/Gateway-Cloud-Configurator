using AutoMapper;
using i4conn.GatewayCloudConfigurationCore.DataAccessLayer.Dtos;
using i4conn.GatewayCloudConfigurationCore.Persistence.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace i4conn.GatewayCloudConfigurationCore.DataAccessLayer.Profiles
{
    public class GroupInterfaceProfile : Profile
    {
        public GroupInterfaceProfile()
        {
            CreateMap<Ts400InterfacceGruppi, InterfaceGroupDto>()
                .ForMember(
                    dest => dest.Id,
                    opt => opt.MapFrom(src => src.IdGruppoInterfacce.Trim())
                )
                .ForMember(
                    dest => dest.Description,
                    opt => opt.MapFrom(src => src.DesGruppoInterfacce.TrimEnd())
                )
                .ForMember(
                    dest => dest.GatewayId,
                    opt => opt.MapFrom(src => src.IdGateway.Trim())
                );

            CreateMap<InterfaceGroupDto, Ts400InterfacceGruppi>()
                .ForMember(
                    dest => dest.IdGruppoInterfacce,
                    opt => opt.MapFrom(src => src.Id.Trim().ToUpper())
                )
                .ForMember(
                    dest => dest.DesGruppoInterfacce,
                    opt => opt.MapFrom(src => src.Description.TrimEnd())
                )
                .ForMember(
                    dest => dest.IdGateway,
                    opt => opt.MapFrom(src => (src.GatewayId != null) ? src.GatewayId.Trim().ToUpper() : string.Empty)
                );
        }
    }
}
