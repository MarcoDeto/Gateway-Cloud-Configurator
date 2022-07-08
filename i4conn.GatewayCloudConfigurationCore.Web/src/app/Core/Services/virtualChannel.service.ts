import { HttpClient } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { Observable } from "rxjs";
import { environment } from "src/environments/environment";
import { Channel } from "../Models/Channel.model";

@Injectable({
  providedIn: 'root'
})
export class VirtualChannelService {

  constructor(
    private http: HttpClient
  ) { }

  GetVirtualChannelsByInterfaceId(interfaceId: string | null): Observable<Channel[]> {
    return this.http.get<Channel[]>(environment.VirtualChannels + '/GetVirtaulValues/' + interfaceId);
  }

  GetVirtualChannels(interfaceId: string | null, channelId: string | null, name: string | null): Observable<Channel[]> {
    return this.http.get<Channel[]>(environment.VirtualChannels + '/GetVirtualValue?interfaceId=' + interfaceId + '&channelId=s' + channelId + '&name' + name);
  }

  Post(entity: Channel): Observable<any> {
    return this.http.post(environment.VirtualChannels + '/Post', entity);
  }

  Put(entity: Channel): Observable<any> {
    return this.http.put(environment.VirtualChannels + '/Put', entity);
  }

  Delete(entity: Channel): Observable<any> {
    return this.http.delete(environment.VirtualChannels + '/Delete?interfaceId=' + entity.interfaceId + '&channelId=' + entity.channelId + '&direction=' + entity.direction);
  }
}
