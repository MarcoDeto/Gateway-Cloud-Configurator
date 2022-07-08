using AutoMapper;
using i4conn.GatewayCloudConfigurationCore.DataAccessLayer.Dtos;
using i4conn.GatewayCloudConfigurationCore.Persistence.Models;

namespace i4conn.GatewayCloudConfigurationCore.DataAccessLayer.Profiles
{
    public class GatewayProfile : Profile
    {
        public GatewayProfile()
        {
            CreateMap<Ts400Gateway, GatewayDto>()
                .ForMember(
                    dest => dest.GatewayId,
                    opt => opt.MapFrom(src => src.IdGateway.Trim())
                )
                .ForMember(
                    dest => dest.Description,
                    opt => opt.MapFrom(src => src.DesGateway.TrimEnd())
                )
                .ForMember(
                    dest => dest.SerialNumber,
                    opt => opt.MapFrom(src => src.Serialnr.Trim())
                )
                .ForMember(
                    dest => dest.GatewayName,
                    opt => opt.MapFrom(src => src.NomeGateway.TrimEnd())
                )
                .ForMember(
                    dest => dest.Location,
                    opt => opt.MapFrom(src => src.Loc.TrimEnd())
                )
                .ForMember(
                    dest => dest.DestinationIp,
                    opt => opt.MapFrom(src => src.DestIp.Trim())
                )
                .ForMember(
                    dest => dest.DestinationPort,
                    opt => opt.MapFrom(src => src.DestPorta.Trim())
                )
                .ForMember(
                    dest => dest.DestinationUser,
                    opt => opt.MapFrom(src => src.DestUser.Trim())
                )
                .ForMember(
                    dest => dest.DestinationPassword,
                    opt => opt.MapFrom(src => src.DestPwd.Trim())
                )
                .ForMember(
                    dest => dest.VersionSupervisor,
                    opt => opt.MapFrom(src => src.VerSupervisor.Trim())
                )
                .ForMember(
                    dest => dest.VersionDevice,
                    opt => opt.MapFrom(src => src.VerDevice.Trim())
                )
                .ForMember(
                    dest => dest.VersionRouter,
                    opt => opt.MapFrom(src => src.VerRouter.Trim())
                )
                .ForMember(
                    dest => dest.VersionRules,
                    opt => opt.MapFrom(src => src.VerRules.Trim())
                )
                .ForMember(
                    dest => dest.FirmwareRoot,
                    opt => opt.MapFrom(src => src.FirmwareRoot.Trim())
                )
                .ForMember(
                    dest => dest.VersionWebapp,
                    opt => opt.MapFrom(src => src.VerWebapp.Trim())
                )
                .ForMember(
                    dest => dest.DestinationSecondaryIp,
                    opt => opt.MapFrom(src => src.DestIpSec.Trim())
                )
                .ForMember(
                    dest => dest.DestinationSecondaryPort,
                    opt => opt.MapFrom(src => src.DestPortaSec.Trim())
                )
                .ForMember(
                    dest => dest.DestinationSecondaryUser,
                    opt => opt.MapFrom(src => src.DestUserSec.Trim())
                )
                .ForMember(
                    dest => dest.DestinationSecondaryPassword,
                    opt => opt.MapFrom(src => src.DestPwdSec.Trim())
                );

            CreateMap<GatewayDto, Ts400Gateway>()
                .ForMember(
                    dest => dest.IdGateway,
                    opt => opt.MapFrom(src => src.GatewayId.Trim().ToUpper())
                )
                .ForMember(
                    dest => dest.DesGateway,
                    opt => opt.MapFrom(src => src.Description.TrimEnd())
                )
                .ForMember(
                    dest => dest.Serialnr,
                    opt => opt.MapFrom(src => src.SerialNumber.Trim())
                )
                .ForMember(
                    dest => dest.NomeGateway,
                    opt => opt.MapFrom(src => src.GatewayName.TrimEnd())
                )
                .ForMember(
                    dest => dest.Loc,
                    opt => opt.MapFrom(src => src.Location.TrimEnd())
                )
                .ForMember(
                    dest => dest.DestIp,
                    opt => opt.MapFrom(src => src.DestinationIp.Trim())
                )
                .ForMember(
                    dest => dest.DestPorta,
                    opt => opt.MapFrom(src => src.DestinationPort.Trim())
                )
                .ForMember(
                    dest => dest.DestUser,
                    opt => opt.MapFrom(src => (src.DestinationUser != null) ? src.DestinationUser.Trim(): string.Empty)
                )
                .ForMember(
                    dest => dest.DestPwd,
                    opt => opt.MapFrom(src => (src.DestinationPassword != null) ? src.DestinationPassword.Trim() : string.Empty)
                )
                .ForMember(
                    dest => dest.VerSupervisor,
                    opt => opt.MapFrom(src => (src.VersionSupervisor != null) ? src.VersionSupervisor.Trim() : string.Empty)
                )
                .ForMember(
                    dest => dest.VerDevice,
                    opt => opt.MapFrom(src => (src.VersionDevice != null) ? src.VersionDevice.Trim() : string.Empty)
                )
                .ForMember(
                    dest => dest.VerRouter,
                    opt => opt.MapFrom(src => (src.VersionRouter != null) ? src.VersionRouter.Trim() : string.Empty)
                )
                .ForMember(
                    dest => dest.VerRules,
                    opt => opt.MapFrom(src => (src.VersionRules != null) ? src.VersionRules.Trim() : string.Empty)
                )
                .ForMember(
                    dest => dest.FirmwareRoot,
                    opt => opt.MapFrom(src => (src.FirmwareRoot != null) ? src.FirmwareRoot.Trim() : string.Empty)
                )
                .ForMember(
                    dest => dest.VerWebapp,
                    opt => opt.MapFrom(src => (src.VersionWebapp != null) ? src.VersionWebapp.Trim() : string.Empty)
                )
                .ForMember(
                    dest => dest.DestIpSec,
                    opt => opt.MapFrom(src => (src.DestinationSecondaryIp != null) ? src.DestinationSecondaryIp.Trim() : string.Empty)
                )
                .ForMember(
                    dest => dest.DestPortaSec,
                    opt => opt.MapFrom(src => (src.DestinationSecondaryPort != null) ? src.DestinationSecondaryPort.Trim() : string.Empty)
                )
                .ForMember(
                    dest => dest.DestUserSec,
                    opt => opt.MapFrom(src => (src.DestinationSecondaryUser != null) ? src.DestinationSecondaryUser.Trim() : string.Empty)
                )
                .ForMember(
                    dest => dest.DestPwdSec,
                    opt => opt.MapFrom(src => (src.DestinationSecondaryPassword != null) ? src.DestinationSecondaryPassword.Trim() : string.Empty)
                );
        }
    }
}
