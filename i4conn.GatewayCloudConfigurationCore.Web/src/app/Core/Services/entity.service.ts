import { HttpClient } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { Observable } from "rxjs";
import { environment } from "src/environments/environment";
import { Entity } from "../Models/Entity.model";

@Injectable({
    providedIn: 'root'
  })
  export class EntityService {
  
    constructor(
      private http: HttpClient
    ) { }
  
      getAll(): Observable<Entity[]> {
          return this.http.get<Entity[]>(environment.Entity + '/Get');
      }
  
      getById(id: string | null): Observable<Entity> {
          return this.http.get<Entity>(environment.Entity + '/Get/' + id);
      }
  
      Add(entity: Entity): Observable<any> {
          return this.http.post(environment.Entity + '/Post', entity);
      }
  
      Edit(entity: Entity): Observable<any> {
          return this.http.put(environment.Entity + '/Put', entity);
      }
  
      Delete(id: string | null): Observable<any> {
          return this.http.delete(environment.Entity + '/Delete/' + id);
      }
  }