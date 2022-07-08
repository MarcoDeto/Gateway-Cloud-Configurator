import { HttpClient } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { Observable } from "rxjs";
import { environment } from "src/environments/environment";
import { TypeEntity } from "../Models/Entity.model";

@Injectable({
    providedIn: 'root'
  })
  export class typeEntityService {

    constructor(
      private http: HttpClient
    ) { }

      getAll(): Observable<TypeEntity[]> {
          return this.http.get<TypeEntity[]>(environment.TypeEntity + '/Get');
      }

      getByEntity(entity: string): Observable<TypeEntity[]> {
        return this.http.get<TypeEntity[]>(environment.TypeEntity + '/GetByEntity?entity=' + entity);
      }

      getByInterfaccia(): Observable<TypeEntity[]> {
        return this.http.get<TypeEntity[]>(environment.TypeEntity + '/GetByEntity?entity=INTERFACCIA');
      }

      getById(id: string | null): Observable<TypeEntity> {
          return this.http.get<TypeEntity>(environment.TypeEntity + '/Get/' + id);
      }

      Add(typeEntity: TypeEntity): Observable<any> {
          return this.http.post(environment.TypeEntity + '/Post', typeEntity);
      }

      Edit(typeEntity: TypeEntity): Observable<any> {
          return this.http.put(environment.TypeEntity + '/Put', typeEntity);
      }

      Delete(id: string | null): Observable<any> {
          return this.http.delete(environment.TypeEntity + '/Delete/' + id);
      }

      TypeInterfacesInputOutputNumber(id: string):   Observable<any> {
        return this.http.get(environment.TypeEntity + '/TypeInterfacesInputOutputNumber?id=' + id);
      }
  }
