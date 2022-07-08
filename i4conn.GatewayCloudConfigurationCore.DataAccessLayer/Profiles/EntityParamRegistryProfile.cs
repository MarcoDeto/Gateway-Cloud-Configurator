using AutoMapper;
using i4conn.GatewayCloudConfigurationCore.DataAccessLayer.Dtos;
using i4conn.GatewayCloudConfigurationCore.Persistence.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace i4conn.GatewayCloudConfigurationCore.DataAccessLayer.Profiles
{
    public class EntityParamRegistryProfile : Profile
    {
        public EntityParamRegistryProfile()
        {
            CreateMap<Ts400ParamEntAnagr, EntityParamRegistryDto>()
                .ForMember(
                    dest => dest.Entity,
                    opt => opt.MapFrom(src => src.Entita.Trim())
                )
                .ForMember(
                    dest => dest.ParamName,
                    opt => opt.MapFrom(src => src.ParamNome.Trim())
                )
                .ForMember(
                    dest => dest.ParamDefaultValue,
                    opt => opt.MapFrom(src => src.ParamValoreDefault.Trim())
                )
                .ForMember(
                    dest => dest.Type,
                    opt => opt.MapFrom(src => src.Tipologia.Trim())
                );

            CreateMap<EntityParamRegistryDto, Ts400ParamEntAnagr>()
                .ForMember(
                    dest => dest.Entita,
                    opt => opt.MapFrom(src => src.Entity.Trim().ToUpper())
                )
                .ForMember(
                    dest => dest.ParamNome,
                    opt => opt.MapFrom(src => src.ParamName.Trim())
                )
                .ForMember(
                    dest => dest.ParamValoreDefault,
                    opt => opt.MapFrom(src => (src.ParamDefaultValue != null) ? src.ParamDefaultValue.Trim() : string.Empty)
                )
                .ForMember(
                    dest => dest.Tipologia,
                    opt => opt.MapFrom(src => (src.Type != null) ? src.Type.Trim().ToUpper() : string.Empty)
                );
        }
    }
}
