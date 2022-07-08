import { HttpClient } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { Observable } from "rxjs";
import { environment } from "src/environments/environment";
import { Channel, ChannelAssociate, AssociableChannel } from '../Models/Channel.model';

@Injectable({
  providedIn: 'root'
})
export class ChannelAssociateService {

  constructor(
    private http: HttpClient
  ) { }

  getAssociableChannels(interfaceId: string, direction: string): Observable<AssociableChannel[]> {
    return this.http.get<AssociableChannel[]>(environment.ChannelAssociate + '/GetAssociableChannels?interfaceId=' + interfaceId + '&direction=' + direction);
  }

  getAssociateChannels(channel: Channel): Observable<ChannelAssociate[]> {
    return this.http.get<ChannelAssociate[]>(environment.ChannelAssociate + '/GetAssociateChannels?interfaceId=' + channel.interfaceId + '&virtualChId=' + channel.channelId);
  }

  post(channel: ChannelAssociate): Observable<any> {
    return this.http.post(environment.ChannelAssociate + '/Post/', channel);
  }

  put(channel: ChannelAssociate): Observable<any> {
    return this.http.put(environment.ChannelAssociate + '/Put/', channel);
  }

// "https://localhost:5001/api/ChannelInterfaceAssociate/Delete?interfaceId=x&channelId=x&virtualCh=x&direction=x"
  delete(channel: ChannelAssociate): Observable<any> {
    return this.http.delete(environment.ChannelAssociate + '/Delete?interfaceId=' + channel.interfaceId
     +'&channelId=' + channel.channelId + '&virtualCh='+ channel.virtualChannelId + '&direction=' + channel.direction);
  }
}
