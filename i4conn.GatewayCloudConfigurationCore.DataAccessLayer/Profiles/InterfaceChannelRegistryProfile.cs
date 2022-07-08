using AutoMapper;
using i4conn.GatewayCloudConfigurationCore.DataAccessLayer.Dtos;
using i4conn.GatewayCloudConfigurationCore.Persistence.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace i4conn.GatewayCloudConfigurationCore.DataAccessLayer.Profiles
{
    public class InterfaceChannelRegistryProfile : Profile
    {
        public InterfaceChannelRegistryProfile()
        {
            CreateMap<Ts400InterfacceCanaliAnagr, InterfaceChannelRegistryDto>()
                .ForMember(
                    dest => dest.ChannelId,
                    opt => opt.MapFrom(src => src.IdCanale.Trim())
                )
                .ForMember(
                    dest => dest.TypologyInterface,
                    opt => opt.MapFrom(src => src.TipologiaInterfaccia.Trim())
                )
                .ForMember(
                    dest => dest.ChannelType,
                    opt => opt.MapFrom(src => src.TipoCanale.Trim())
                )
                .ForMember(
                    dest => dest.Description,
                    opt => opt.MapFrom(src => src.Descrizione.Trim())
                )
                .ForMember(
                    dest => dest.Direction,
                    opt => opt.MapFrom(src => src.Direzione.Trim())
                );
        }
    }
}
