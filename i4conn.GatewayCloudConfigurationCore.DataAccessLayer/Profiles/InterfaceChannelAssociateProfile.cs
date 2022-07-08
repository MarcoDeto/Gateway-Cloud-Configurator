using AutoMapper;
using i4conn.GatewayCloudConfigurationCore.DataAccessLayer.Dtos;
using i4conn.GatewayCloudConfigurationCore.Persistence.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace i4conn.GatewayCloudConfigurationCore.DataAccessLayer.Profiles
{
    public class InterfaceChannelAssociateProfile : Profile
    {
        public InterfaceChannelAssociateProfile()
        {
            CreateMap<Ts400InterfacceCanaliAssociati, InterfaceChannelAssociateDto>()
                .ForMember(
                    dest => dest.InterfaceId,
                    opt => opt.MapFrom(src => src.IdInterfaccia.Trim())
                )
                .ForMember(
                    dest => dest.VirtualChannelId,
                    opt => opt.MapFrom(src => src.IdCanaleVirtuale.Trim())
                )
                .ForMember(
                    dest => dest.ChannelId,
                    opt => opt.MapFrom(src => src.IdCanale.Trim())
                )
                .ForMember(
                    dest => dest.Direction,
                    opt => opt.MapFrom(src => src.Direzione.Trim())
                );

            CreateMap<InterfaceChannelAssociateDto, Ts400InterfacceCanaliAssociati>()
                .ForMember(
                    dest => dest.IdInterfaccia,
                    opt => opt.MapFrom(src => src.InterfaceId.Trim())
                )
                .ForMember(
                    dest => dest.IdCanaleVirtuale,
                    opt => opt.MapFrom(src => src.VirtualChannelId.Trim())
                )
                .ForMember(
                    dest => dest.IdCanale,
                    opt => opt.MapFrom(src => src.ChannelId.Trim())
                )
                .ForMember(
                    dest => dest.Direzione,
                    opt => opt.MapFrom(src => src.Direction.Trim().ToUpper())
                );
        }
    }
}
