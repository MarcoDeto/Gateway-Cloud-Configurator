import { Injectable } from '@angular/core';
import { HttpClient, HttpParams, HttpHeaders } from '@angular/common/http';
import { Observable, throwError } from 'rxjs';
import { catchError } from 'rxjs/operators';
import { environment } from 'src/environments/environment';
import { Response } from 'src/app/Authentication/Models/response.model';
import { Gateway } from '../Models/Gateway.model';

@Injectable({
  providedIn: 'root'
})
export class GatewayService {

  constructor(
    private http: HttpClient
  ) { }

    getAll(): Observable<Gateway[]> {
        return this.http.get<Gateway[]>(environment.Gateway + '/Get');
    }

    getById(id: string | null): Observable<Gateway> {
        return this.http.get<Gateway>(environment.Gateway + '/Get/' + id);
    }

    Add(gateway: Gateway): Observable<any> {
        return this.http.post(environment.Gateway + '/Post', gateway);
    }

    Edit(gateway: Gateway): Observable<any> {
        return this.http.put(environment.Gateway + '/Put', gateway);
    }

    Delete(id: string | null): Observable<any> {
        return this.http.delete(environment.Gateway + '/Delete/' + id);
    }

    getFirmwares(path: string): Observable<any> {
        return this.http.get(environment.Firmware + path);
    }
}