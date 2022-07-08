import { HttpClient } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { Observable } from "rxjs";
import { environment } from "src/environments/environment";
import { Interface } from "../Models/Interface.model";

@Injectable({
    providedIn: 'root'
})
export class InterfaceService {

    constructor(
        private http: HttpClient
    ) { }

    getAll(): Observable<Interface[]> {
        return this.http.get<Interface[]>(environment.Interfaces + '/Get');
    }

    getByGatewayId(gatewayId: string | null): Observable<Interface[]> {
        return this.http.get<Interface[]>(environment.Interfaces + '/GetAllByGateway?gatewayId=' + gatewayId);
    }

    getByGroupId(groupId: string | null): Observable<Interface[]> {
        return this.http.get<Interface[]>(environment.Interfaces + '/GetAllByGateway?groupId=' + groupId);
    }

    getAvailableAdaptersByGroup(groupId: string | null): Observable<Interface[]> {
        return this.http.get<Interface[]>(environment.Interfaces + '/GetAvailableAdaptersByGroup?groupId=' + groupId);
    }

    getById(id: string | null): Observable<Interface> {
        return this.http.get<Interface>(environment.Interfaces + '/Get/' + id);
    }

    add(interfaccia: Interface): Observable<any> {
        console.log(interfaccia);
        return this.http.post(environment.Interfaces + '/Post', interfaccia);
    }

    edit(interfaccia: Interface): Observable<any> {
        console.log(interfaccia);
        return this.http.put(environment.Interfaces + '/Put', interfaccia);
    }

    delete(id: string | undefined): Observable<any> {
        return this.http.delete(environment.Interfaces + '/Delete/' + id);
    }
}
