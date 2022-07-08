using AutoMapper;
using i4conn.GatewayCloudConfigurationCore.DataAccessLayer.Dtos;
using i4conn.GatewayCloudConfigurationCore.Persistence.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace i4conn.GatewayCloudConfigurationCore.DataAccessLayer.Profiles
{
    public class InterfaceProfile : Profile
    {
        public InterfaceProfile()
        {
            CreateMap<Ts400Interfacce, InterfaceDto>()
                .ForMember(
                    dest => dest.InterfaceId,
                    opt => opt.MapFrom(src => src.IdInterfaccia.Trim())
                )
                .ForMember(
                    dest => dest.InterfaceDescription,
                    opt => opt.MapFrom(src => src.DesInterfaccia.TrimEnd())
                )
                .ForMember(
                    dest => dest.TerminalIp,
                    opt => opt.MapFrom(src => src.IpTerminale.Trim())
                )
                .ForMember(
                    dest => dest.TerminalPort,
                    opt => opt.MapFrom(src => src.PortaTerminale.Trim())
                )
                .ForMember(
                    dest => dest.DeviceId,
                    opt => opt.MapFrom(src => src.IdDispositivo)
                )
                .ForMember(
                    dest => dest.Router,
                    opt => opt.MapFrom(src => src.Router.Trim())
                )
                .ForMember(
                    dest => dest.InputChannelNumber,
                    opt => opt.MapFrom(src => src.NumCanaliI)
                )
                .ForMember(
                    dest => dest.OutputChannelNumber,
                    opt => opt.MapFrom(src => src.NumCanaliU)
                )
                .ForMember(
                    dest => dest.InterfaceGroupId,
                    opt => opt.MapFrom(src => src.IdGruppoInterfacce.Trim())
                )
                .ForMember(
                    dest => dest.InterfaceGroupDescription,
                    opt => opt.MapFrom(src => src.DesGruppoInterfacce.TrimEnd())
                )
                .ForMember(
                    dest => dest.LastInterrogation,
                    opt => opt.MapFrom(src => src.UltimaInterrogazione)
                )
                .ForMember(
                    dest => dest.TypologyInterface,
                    opt => opt.MapFrom(src => src.InterfacciaContapezzi.Trim())
                )
                .ForMember(
                    dest => dest.Coordinator,
                    opt => opt.MapFrom(src => src.Coordinatore)
                );

            CreateMap<InterfaceDto, Ts400Interfacce> ()
                .ForMember(
                    dest => dest.IdInterfaccia,
                    opt => opt.MapFrom(src => src.InterfaceId.Trim())
                )
                .ForMember(
                    dest => dest.DesInterfaccia,
                    opt => opt.MapFrom(src => src.InterfaceDescription.TrimEnd())
                )
                .ForMember(
                    dest => dest.IpTerminale,
                    opt => opt.MapFrom(src => (src.TerminalIp != null) ? src.TerminalIp.Trim() : string.Empty)
                )
                .ForMember(
                    dest => dest.PortaTerminale,
                    opt => opt.MapFrom(src => (src.TerminalPort != null) ? src.TerminalPort.Trim() : string.Empty)
                )
                .ForMember(
                    dest => dest.IdDispositivo,
                    opt => opt.MapFrom(src => src.DeviceId)
                )
                .ForMember(
                    dest => dest.Router,
                    opt => opt.MapFrom(src => (src.Router != null) ? src.Router.Trim() : string.Empty)
                )
                .ForMember(
                    dest => dest.NumCanaliI,
                    opt => opt.MapFrom(src => src.InputChannelNumber)
                )
                .ForMember(
                    dest => dest.NumCanaliU,
                    opt => opt.MapFrom(src => src.OutputChannelNumber)
                )
                .ForMember(
                    dest => dest.IdGruppoInterfacce,
                    opt => opt.MapFrom(src => (src.InterfaceGroupId != null) ? src.InterfaceGroupId.Trim().ToUpper() : string.Empty)
                )
                .ForMember(
                    dest => dest.DesGruppoInterfacce,
                    opt => opt.MapFrom(src => (src.InterfaceGroupDescription != null) ? src.InterfaceGroupDescription.TrimEnd() : string.Empty)
                )
                .ForMember(
                    dest => dest.UltimaInterrogazione,
                    opt => opt.MapFrom(src => src.LastInterrogation)
                )
                .ForMember(
                    dest => dest.InterfacciaContapezzi,
                    opt => opt.MapFrom(src => src.TypologyInterface.Trim().ToUpper())
                )
                .ForMember(
                    dest => dest.Coordinatore,
                    opt => opt.MapFrom(src => (src.Coordinator != null) ? src.Coordinator : string.Empty)
                );
        }
    }
}
