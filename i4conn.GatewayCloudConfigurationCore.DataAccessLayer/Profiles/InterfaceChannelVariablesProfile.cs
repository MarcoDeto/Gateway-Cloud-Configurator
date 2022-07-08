using AutoMapper;
using i4conn.GatewayCloudConfigurationCore.DataAccessLayer.Dtos;
using i4conn.GatewayCloudConfigurationCore.Persistence.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace i4conn.GatewayCloudConfigurationCore.DataAccessLayer.Profiles
{
    public class InterfaceChannelVariablesProfile : Profile
    {
        public InterfaceChannelVariablesProfile()
        {
            CreateMap<Ts400InterfacceCanaliVariabili, InterfaceChannelVariablesDto>()
                .ForMember(
                    dest => dest.InterfaceId,
                    opt => opt.MapFrom(src => src.IdInterfaccia.Trim())
                )
                .ForMember(
                    dest => dest.ChannelId,
                    opt => opt.MapFrom(src => src.IdCanale.Trim())
                )
                .ForMember(
                    dest => dest.Direction,
                    opt => opt.MapFrom(src => src.Direzione.Trim())
                )
                .ForMember(
                    dest => dest.Name,
                    opt => opt.MapFrom(src => src.Nome.Trim())
                )
                .ForMember(
                    dest => dest.Group,
                    opt => opt.MapFrom(src => src.Gruppo.Trim())
                )
                .ForMember(
                    dest => dest.Key,
                    opt => opt.MapFrom(src => src.Chiave.Trim())
                );

            CreateMap<InterfaceChannelVariablesDto, Ts400InterfacceCanaliVariabili>()
                .ForMember(
                    dest => dest.IdInterfaccia,
                    opt => opt.MapFrom(src => src.InterfaceId.Trim())
                )
                .ForMember(
                    dest => dest.IdCanale,
                    opt => opt.MapFrom(src => src.ChannelId.Trim())
                )
                .ForMember(
                    dest => dest.Direzione,
                    opt => opt.MapFrom(src => src.Direction.Trim().ToUpper())
                )
                .ForMember(
                    dest => dest.Nome,
                    opt => opt.MapFrom(src => src.Name.Trim())
                )
                .ForMember(
                    dest => dest.Gruppo,
                    opt => opt.MapFrom(src => (src.Group != null) ? src.Group.Trim() : string.Empty)
                )
                .ForMember(
                    dest => dest.Chiave,
                    opt => opt.MapFrom(src => src.Key.Trim())
                );
        }
    }
}
