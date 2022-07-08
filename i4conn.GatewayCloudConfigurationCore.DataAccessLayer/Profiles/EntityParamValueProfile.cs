using AutoMapper;
using i4conn.GatewayCloudConfigurationCore.DataAccessLayer.Dtos;
using i4conn.GatewayCloudConfigurationCore.DataAccessLayer.Models;
using i4conn.GatewayCloudConfigurationCore.Persistence.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace i4conn.GatewayCloudConfigurationCore.DataAccessLayer.Profiles
{
    public class EntityParamProfile : Profile
    {
        public EntityParamProfile()
        {
            CreateMap<EntityParam, Ts400ParamEntValori>()
                .ForMember(
                    dest => dest.Entita,
                    opt => opt.MapFrom(src => src.Entity.Trim().ToUpper())
                )
                .ForMember(
                    dest => dest.IdEntita,
                    opt => opt.MapFrom(src => src.EntityId.Trim().ToUpper())
                )
                .ForMember(
                    dest => dest.ParamNome,
                    opt => opt.MapFrom(src => src.ParamName.Trim())
                )
                .ForMember(
                    dest => dest.ParamValore,
                    opt => opt.MapFrom(src => src.ParamValue.Trim())
                );

            CreateMap<Ts400ParamEntValori, EntityParam>()
                .ForMember(
                    dest => dest.Entity,
                    opt => opt.MapFrom(src => src.Entita.Trim())
                )
                .ForMember(
                    dest => dest.EntityId,
                    opt => opt.MapFrom(src => (src.IdEntita != null) ? src.IdEntita.Trim() : string.Empty)
                )
                .ForMember(
                    dest => dest.ParamName,
                    opt => opt.MapFrom(src => src.ParamNome.Trim())
                )
                .ForMember(
                    dest => dest.ParamValue,
                    opt => opt.MapFrom(src => src.ParamValore.Trim())
                );
        }
    }
}
