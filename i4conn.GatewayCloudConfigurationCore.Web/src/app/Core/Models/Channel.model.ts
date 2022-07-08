export class Channel {
  constructor(
      public interfaceId: string,
      public channelId: string,
      public channelType: string,
      public description: string,
      public direction: string,
      public flgVirtual: boolean,
      public ruleId: string,
      public destination: string,
      public originChannelId: string,
      public allowEditChannel: boolean,
      public allowEditVariable:	boolean,
      public variables: VariableChannel[]
  ) {}
}

export class VariableChannel {
  constructor(
      public interfaceId: string,
      public channelId: string,
      public direction: string,
      public name: string,
      public group: string,
      public key: string,
  ) {}
}

export class AssociableChannel {
  constructor(
    public channelId: string,
    public typologyInterface: string,
    public channelType: string,
    public description: string,
    public direction: string
  ) {}
}

export class ChannelAssociate {
  constructor(
    public interfaceId: string,
    public virtualChannelId: string,
    public channelId: string,
    public direction: string,
    public flgAbilitaOriginale: boolean,
    public chDescription: string,
    public virtualChDesctiption: string,
    public chType: string,
    public virtualChType: string
  ) {}
}
