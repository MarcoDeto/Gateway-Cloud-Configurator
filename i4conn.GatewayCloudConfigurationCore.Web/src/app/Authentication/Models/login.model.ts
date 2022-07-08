export interface LoginModel{
    username: string;
    password: string;
}
  
export class LoginResponse {
    constructor(
      public token: string,
      public expiration: string
    ) { }
}
  