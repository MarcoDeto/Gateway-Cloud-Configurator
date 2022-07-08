import { HttpClient } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { Observable } from "rxjs";
import { environment } from "src/environments/environment";
import { Channel } from "../Models/Channel.model";

@Injectable({
  providedIn: 'root'
})
export class ChannelService {

  constructor(
    private http: HttpClient
  ) { }

  getAllByInterfaceId(interfaceId: string | null): Observable<Channel[]> {
    return this.http.get<Channel[]>(environment.Channels + '/GetValuesByInterface/' + interfaceId);
  }

  put(channel: Channel): Observable<any> {
    return this.http.put(environment.Channels + '/Put/', channel);
  }

  delete(channel: Channel): Observable<any> {
    return this.http.delete(environment.Channels + '/Delete?interfaceId=' + channel.interfaceId +'&channelId=' + channel.channelId + '&direction=' + channel.direction);
  }
}
