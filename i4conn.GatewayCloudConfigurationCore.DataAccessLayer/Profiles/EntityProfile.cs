using AutoMapper;
using i4conn.GatewayCloudConfigurationCore.DataAccessLayer.Dtos;
using i4conn.GatewayCloudConfigurationCore.Persistence.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace i4conn.GatewayCloudConfigurationCore.DataAccessLayer.Profiles
{
    public class EntityProfile : Profile
    {
        public EntityProfile()
        {
            CreateMap<Ts400Entitum, EntityDto>()
                .ForMember(
                    dest => dest.Name,
                    opt => opt.MapFrom(src => src.Entita.Trim())
                )
                .ForMember(
                    dest => dest.Description,
                    opt => opt.MapFrom(src => src.DesEntita.Trim())
                );

            CreateMap<EntityDto, Ts400Entitum>()
                .ForMember(
                    dest => dest.Entita,
                    opt => opt.MapFrom(src => src.Name.Trim().ToUpper())
                )
                .ForMember(
                    dest => dest.DesEntita,
                    opt => opt.MapFrom(src => src.Description.Trim())
                );
        }
    }
}
