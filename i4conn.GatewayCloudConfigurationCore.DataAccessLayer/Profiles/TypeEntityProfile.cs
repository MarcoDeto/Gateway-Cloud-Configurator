using AutoMapper;
using i4conn.GatewayCloudConfigurationCore.DataAccessLayer.Dtos;
using i4conn.GatewayCloudConfigurationCore.Persistence.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace i4conn.GatewayCloudConfigurationCore.DataAccessLayer.Profiles
{
    public class TypeEntityProfile : Profile
    {
        public TypeEntityProfile()
        {
            CreateMap<Ts400EntitaTipologium, TypeEntityDto>()
                .ForMember(
                    dest => dest.Id,
                    opt => opt.MapFrom(src => src.IdTipo.Trim())
                )
                .ForMember(
                    dest => dest.Description,
                    opt => opt.MapFrom(src => src.DesTipo.TrimEnd())
                )
                .ForMember(
                    dest => dest.Entity,
                    opt => opt.MapFrom(src => src.Entita.Trim())
                )
                .ForMember(
                    dest => dest.Param1,
                    opt => opt.MapFrom(src => src.Par01.TrimEnd())
                )
                .ForMember(
                    dest => dest.Param2,
                    opt => opt.MapFrom(src => src.Par02.TrimEnd())
                )
                .ForMember(
                    dest => dest.Param3,
                    opt => opt.MapFrom(src => src.Par03.Trim())
                );

            CreateMap<TypeEntityDto, Ts400EntitaTipologium>()
                .ForMember(
                    dest => dest.IdTipo,
                    opt => opt.MapFrom(src => src.Id.Trim().ToUpper())
                )
                .ForMember(
                    dest => dest.DesTipo,
                    opt => opt.MapFrom(src => src.Description.TrimEnd())
                )
                .ForMember(
                    dest => dest.Entita,
                    opt => opt.MapFrom(src => src.Entity.Trim().ToUpper())
                )
                .ForMember(
                    dest => dest.Par01,
                    opt => opt.MapFrom(src => (src.Param1 != null) ? src.Param1.TrimEnd() : string.Empty)
                )
                .ForMember(
                    dest => dest.Par02,
                    opt => opt.MapFrom(src => (src.Param2 != null) ?  src.Param2.TrimEnd() : string.Empty)
                )
                .ForMember(
                    dest => dest.Par03,
                    opt => opt.MapFrom(src => (src.Param3 != null) ?  src.Param3.Trim() : string.Empty)
                );
        }
    }
}
