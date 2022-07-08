using AutoMapper;
using i4conn.GatewayCloudConfigurationCore.DataAccessLayer.Dtos;
using i4conn.GatewayCloudConfigurationCore.Persistence.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace i4conn.GatewayCloudConfigurationCore.DataAccessLayer.Profiles
{
    public class InterfaceChannelValuesProfile : Profile
    {
        public InterfaceChannelValuesProfile()
        {
            CreateMap<Ts400InterfacceCanaliValori, InterfaceChannelValuesDto>()
                .ForMember(
                    dest => dest.InterfaceId,
                    opt => opt.MapFrom(src => src.IdInterfaccia.Trim())
                )
                .ForMember(
                    dest => dest.ChannelId,
                    opt => opt.MapFrom(src => src.IdCanale.Trim())
                )
                .ForMember(
                    dest => dest.ChannelType,
                    opt => opt.MapFrom(src => src.TipoCanale.Trim())
                )
                .ForMember(
                    dest => dest.Description,
                    opt => opt.MapFrom(src => src.Descrizione.TrimEnd())
                )
                .ForMember(
                    dest => dest.Direction,
                    opt => opt.MapFrom(src => src.Direzione.Trim())
                )
                .ForMember(
                    dest => dest.RuleId,
                    opt => opt.MapFrom(src => src.IdRegola.Trim())
                )
                .ForMember(
                    dest => dest.Destination,
                    opt => opt.MapFrom(src => src.Destination.Trim())
                )
                .ForMember(
                    dest => dest.OriginChannelId,
                    opt => opt.MapFrom(src => src.IdCanaleOrigine.Trim())
                );

            CreateMap<InterfaceChannelValuesDto, Ts400InterfacceCanaliValori>()
                .ForMember(
                    dest => dest.IdInterfaccia,
                    opt => opt.MapFrom(src => src.InterfaceId.Trim())
                )
                .ForMember(
                    dest => dest.IdCanale,
                    opt => opt.MapFrom(src => src.ChannelId.Trim())
                )
                .ForMember(
                    dest => dest.TipoCanale,
                    opt => opt.MapFrom(src => src.ChannelType.Trim())
                )
                .ForMember(
                    dest => dest.Descrizione,
                    opt => opt.MapFrom(src => src.Description.TrimEnd())
                )
                .ForMember(
                    dest => dest.Direzione,
                    opt => opt.MapFrom(src => src.Direction.Trim().ToUpper())
                )
                .ForMember(
                    dest => dest.IdRegola,
                    opt => opt.MapFrom(src => (src.RuleId != null) ? src.RuleId.Trim() : string.Empty)
                )
                .ForMember(
                    dest => dest.Destination,
                    opt => opt.MapFrom(src => (src.Destination != null) ? src.Destination.Trim() : string.Empty)
                )
                .ForMember(
                    dest => dest.IdCanaleOrigine,
                    opt => opt.MapFrom(src => (src.OriginChannelId != null) ? src.OriginChannelId.Trim() : string.Empty)
                );
        }
    }
}
