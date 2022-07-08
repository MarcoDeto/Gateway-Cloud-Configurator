import { HttpClient } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { Observable } from "rxjs";
import { environment } from "src/environments/environment";
import { Channel } from "../Models/Channel.model";
import { EntityParam } from '../Models/ParamInterface.model';
import { RuleRequest } from '../Models/RuleRequest.model';

@Injectable({
  providedIn: 'root'
})
export class RuleService {

  constructor(
    private http: HttpClient
  ) { }

  GetRuleParams(channel: Channel): Observable<EntityParam[]> {
    return this.http.get<EntityParam[]>(environment.ParamValue+'/GetRuleParams?interfaceId='+channel.interfaceId+'&direction='+channel.direction+'&virtualCh='+channel.channelId);
  }

  PostRule(request: RuleRequest): Observable<any> {
    return this.http.post(environment.ParamValue + '/PostRule', request);
  }

  PutRule(request: RuleRequest): Observable<any> {
    return this.http.put(environment.ParamValue + '/PutRule', request);
  }
}
