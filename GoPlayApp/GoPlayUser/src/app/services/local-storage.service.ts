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

  public getToken(): Token{
    const token = localStorage.getItem('Authorization');
    return token !== null ? JSON.parse(token) : null;
  }

  public getUser() {
    let user = localStorage.getItem('User');
    return user !== null ? JSON.parse(user) : null;
  }
}
