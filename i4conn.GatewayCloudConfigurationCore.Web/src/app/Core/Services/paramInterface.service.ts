import { Injectable } from '@angular/core';
import { HttpClient, HttpParams, HttpHeaders } from '@angular/common/http';
import { Observable, throwError } from 'rxjs';
import { catchError } from 'rxjs/operators';
import { environment } from 'src/environments/environment';
import { Response } from 'src/app/Authentication/Models/response.model';
import { EntityParam, ParamInterface } from '../Models/ParamInterface.model';

@Injectable({
  providedIn: 'root'
})
export class ParamInterfaceService {

  constructor(
    private http: HttpClient
  ) { }

  getAll(): Observable<ParamInterface[]> {
    return this.http.get<ParamInterface[]>(environment.ParamRegistry + '/Get');
  }

  getByName(name: string | null): Observable<ParamInterface> {
    return this.http.get<ParamInterface>(environment.ParamRegistry + '/GetByName?name=' + name);
  }

  getByEntity(entity: string | null): Observable<ParamInterface> {
    return this.http.get<ParamInterface>(environment.ParamRegistry + '/GetByEntity?entity=' + entity);
  }

  getByType(type: string | null): Observable<ParamInterface[]> {
    return this.http.get<ParamInterface[]>(environment.ParamRegistry + '/GetByType?type=' + type);
  }

  getInterfaceParams(interfaceId: string | undefined): Observable<EntityParam[]> {
    return this.http.get<EntityParam[]>(environment.ParamValue + '/GetInterfaceParams/' + interfaceId);
  }

  Add(entity: ParamInterface): Observable<any> {
    return this.http.post(environment.ParamRegistry + '/Post', entity);
  }

  Edit(entity: EntityParam): Observable<any> {
    return this.http.post(environment.ParamValue + '/PostInterfaceParam', entity);
  }

  Default(entity: EntityParam): Observable<any> {
    return this.http.delete(environment.ParamValue + '/Delete?paramName='+entity.paramName+'&entity='+entity.entity+'&entityId='+entity.entityId);
  }

  Delete(entity: EntityParam): Observable<any> {
    return this.http.delete(environment.ParamRegistry + '/Delete?paramName='+entity.paramName+'&entity='+entity.entity+'&type='+entity.type);
  }
}
