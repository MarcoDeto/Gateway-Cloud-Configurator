using AutoMapper;
using i4conn.GatewayCloudConfigurationCore.DataAccessLayer.Dtos;
using i4conn.GatewayCloudConfigurationCore.Persistence.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace i4conn.GatewayCloudConfigurationCore.DataAccessLayer.Profiles
{
    public class ChannelTypeProfile : Profile
    {
        public ChannelTypeProfile()
        {
            CreateMap<Ts400TipiCanale, ChannelTypeDto>()
                .ForMember(
                    dest => dest.Type,
                    opt => opt.MapFrom(src => src.TipoCanale.Trim())
                )
                .ForMember(
                    dest => dest.Description,
                    opt => opt.MapFrom(src => src.DesCanale.TrimEnd())
                )
                .ForMember(
                    dest => dest.Topic,
                    opt => opt.MapFrom(src => src.Topic.TrimEnd())
                );

            CreateMap<ChannelTypeDto, Ts400TipiCanale>()
                .ForMember(
                    dest => dest.TipoCanale,
                    opt => opt.MapFrom(src => src.Type.Trim().ToUpper())
                )
                .ForMember(
                    dest => dest.DesCanale,
                    opt => opt.MapFrom(src => src.Description.TrimEnd())
                )
                .ForMember(
                    dest => dest.Topic,
                    opt => opt.MapFrom(src => src.Topic.TrimEnd())
                );
        }
    }
}
