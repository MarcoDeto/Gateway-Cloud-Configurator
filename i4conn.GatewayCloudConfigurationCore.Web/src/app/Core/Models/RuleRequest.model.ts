import { EntityParam } from './ParamInterface.model';

export class RuleRequest
{
  constructor(
    public content: EntityParam,
    public interfaceId: string,
    public direction: string,
    public virtualCh: string,
  ) {}
}
