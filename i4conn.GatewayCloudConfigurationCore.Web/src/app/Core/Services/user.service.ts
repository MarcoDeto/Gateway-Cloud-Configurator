import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable, BehaviorSubject } from 'rxjs';
import { Router } from '@angular/router';
import { environment } from 'src/environments/environment';
import * as moment from "moment";
import { LoginModel, LoginResponse } from '../../Authentication/Models/login.model';


@Injectable({
  providedIn: 'root'
})
export class UserService {

  private _userId: BehaviorSubject<string> = new BehaviorSubject<string>('');
  public userId: Observable<string> = this._userId.asObservable();

  private _userUsername: BehaviorSubject<string> = new BehaviorSubject<string>('');
  public userUsername: Observable<string> = this._userUsername.asObservable();

  private _expiration: BehaviorSubject<string> = new BehaviorSubject<string>('');
  public expiration: Observable<string> = this._expiration.asObservable();

  private token: BehaviorSubject<string> = new BehaviorSubject<string>('');
  public Token: Observable<string> = this.token.asObservable();

  private isAuthenticated: BehaviorSubject<boolean> = new BehaviorSubject<boolean>(false);
  public IsAuthenticated: Observable<boolean> = this.isAuthenticated.asObservable();

  constructor(private http: HttpClient, private router: Router) { }

  login(data: LoginModel) {
    return this.http.post<LoginModel>(environment.Login, data);
  }

  setToken(data: any) {
    this.token.next(data);
    let payLoad = JSON.parse(window.atob(data.split('.')[1]));
    this._userId.next(payLoad.UserId);
    this._userUsername.next(payLoad.Name);
    this._expiration.next(data.expiration);
  }

  removeToken() { this.token.next('')}

  yes() { this.isAuthenticated.next(true); }
  no() { this.isAuthenticated.next(false); }

}
