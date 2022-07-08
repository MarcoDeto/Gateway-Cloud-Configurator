import { Interface } from "./Interface.model";

export class GroupWithAvailable {
    constructor (
      public id: string,
      public description: string,
      public gatewayId: string,
      public interfaces: Interface[],
      public availableAdapters: Interface[]
    ) {}
}

export class interfaceGroup {
  constructor (
      public id: string,
      public description: string,
      public gatewayId: string,
      public interfaces: Interface[]
  ) {}
}
