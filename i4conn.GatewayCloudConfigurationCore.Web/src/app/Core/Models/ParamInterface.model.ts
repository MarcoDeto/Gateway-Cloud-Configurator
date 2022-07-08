
export class ParamInterface {
  constructor(
      public entity: string,
      public paramName: string,
      public paramDefaultValue: string,
      public type: string,
  ) {}
}

export class EntityParam
{
  constructor(
    public entity: string,
    public paramName: string,
    public paramDefaultValue: string,
    public type: string,
    public paramValue: string,
    public entityId: string,
    public useDefault: boolean
  ) {}
}
