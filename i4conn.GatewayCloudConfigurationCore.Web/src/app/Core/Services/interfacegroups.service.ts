import { HttpClient } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { Observable } from "rxjs";
import { environment } from "src/environments/environment";
import { interfaceGroup } from "../Models/InterfaceGroup.model";

@Injectable({
    providedIn: 'root'
})
export class InterfaceGroupService {

    constructor(
        private http: HttpClient
    ) { }

    getAll(): Observable<interfaceGroup[]> {
        return this.http.get<interfaceGroup[]>(environment.InterfaceGroup + '/Get');
    }

    getById(groupId: string | null): Observable<interfaceGroup> {
      return this.http.get<interfaceGroup>(environment.InterfaceGroup + '/Get/' + groupId);
    }

    getByGatewayId(gatewayId: string | null): Observable<interfaceGroup[]> {
        return this.http.get<interfaceGroup[]>(environment.InterfaceGroup + '/GetByGateway?idGateway=' + gatewayId);
    }

    Add(interfaceGroup: interfaceGroup): Observable<any> {
        return this.http.post(environment.InterfaceGroup + '/Post', interfaceGroup);
    }

    Edit(gateway: interfaceGroup): Observable<any> {
        return this.http.put(environment.InterfaceGroup + '/Put', gateway);
    }

    Delete(id: string | null): Observable<any> {
        return this.http.delete(environment.InterfaceGroup + '/Delete/' + id);
    }

    DeleteWithInterfaces(id: string | null): Observable<any> {
        return this.http.delete(environment.InterfaceGroup + '/DeleteWithInterfaces/' + id);
    }
}
