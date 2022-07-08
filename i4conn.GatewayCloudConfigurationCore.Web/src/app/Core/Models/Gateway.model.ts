export class Gateway {
    constructor(
        public gatewayId: string,
        public description: string,
        public serialNumber: string,
        public gatewayName: string,
        public location: string,
        public destinationIp: string,
        public destinationPort: string,
        public destinationUser: string,
        public destinationPassword: string,
        public versionSupervisor: string,
        public versionDevice: string,
        public versionRouter: string,
        public versionRules: string,
        public firmwareRoot: string,
        public versionWebapp: string,
        public destinationSecondaryIp: string,
        public destinationSecondaryPort: string,
        public destinationSecondaryUser: string,
        public destinationSecondaryPassword: string,
        public counterAdapters: number,
        public counterDevices: number
    ) {}
}