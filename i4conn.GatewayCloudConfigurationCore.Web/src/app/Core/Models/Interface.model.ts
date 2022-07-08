export class Interface {
    constructor(
        public interfaceId: string,
        public interfaceDescription: string,
        public terminalIp: string,
        public terminalPort: string,
        public deviceId: number,
        public router: string,
        public inputChannelNumber: number,
        public outputChannelNumber: number,
        public interfaceGroupId: string,
        public interfaceGroupDescription: string,
        public lastInterrogation: string,
        public typologyInterface: string,
        public coordinator: string
    ) {}
}
