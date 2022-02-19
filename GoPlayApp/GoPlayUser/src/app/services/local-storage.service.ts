import { Injectable } from '@angular/core';
import { Token } from '../models/Token';
import { User } from '../models/User';

@Injectable({
  providedIn: 'root'
})
export class LocalStorageService {

  constructor() { }

  public setToken(token: Token) {
    localStorage.setItem('Authorization', JSON.stringify(token));
  }

  public setUser(user: User) {
    localStorage.setItem('User', JSON.stringify(user));
  }

  public getToken() {
    let token : string | undefined = JSON.parse(localStorage.getItem('Authorization')!);
    return token == null ? null : token;
  }

  public getUser() {
    let user : string | undefined = JSON.parse(localStorage.getItem('User')!);
    return user == null ? null : user;
  }
}
