import { HttpClient } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { Observable } from "rxjs";
import { environment } from "src/environments/environment";
import { VariableChannel, Channel } from '../Models/Channel.model';
import { Interface } from '../Models/Interface.model';

@Injectable({
  providedIn: 'root'
})
export class ChannelVariablesService {

  constructor(
    private http: HttpClient
  ) { }

  GetVariable(entity: Channel): Observable<VariableChannel[]> {
    return this.http.get<VariableChannel[]>(environment.ChannelVariables + '/GetVariablesByInterface?interfaceId=' + entity.interfaceId + '&channelId=' + entity.channelId + '&direction=' + entity.direction);
  }

  PostVariable(entity: VariableChannel): Observable<any> {
    return this.http.post(environment.ChannelVariables + '/PostVariable', entity);
  }

  PutVariable(entity: VariableChannel): Observable<any> {
    return this.http.put(environment.ChannelVariables + '/PutVariable', entity);
  }

  DeleteVariable(entity: VariableChannel): Observable<any> {
    return this.http.delete(environment.ChannelVariables + '/DeleteVariable?interfaceId=' + entity.interfaceId + '&channelId=' + entity.channelId + '&name=' + entity.name);
  }
}
