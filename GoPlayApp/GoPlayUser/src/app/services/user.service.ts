import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Router } from '@angular/router';
import { Observable, ReplaySubject } from 'rxjs';
import { map } from 'rxjs/operators';
import { Token } from '../models/Token';
import { User } from '../models/User';
import { LocalStorageService } from './local-storage.service';

@Injectable({
  providedIn: 'root'
})
export class UserServiceService {
  accUrl = "https://localhost:7170/users";

  public user: User;
  public mutedUntil: Date;

  constructor(private http: HttpClient,
    private storageService: LocalStorageService)
    {
      if(this.storageService.getUser() != null) this.user = this.storageService.getUser();
    }

  register(model: any) {
    return this.http.post(this.accUrl + '/register', model).pipe(
      map((response: any) => {
        const user = response;
        const token = response.token;
        if (user) {
          this.storageService.setUser(user);
          this.storageService.setToken(token);
          this.user = user;
        }
        return user;
      })
    );
  }


  login(model: any): Observable<Token> {
    return this.http.post<Token>(this.accUrl + '/login', model).pipe(
      map((response: any) => {
        const user = response;
        const token = response.token;
        if (user) {
          this.storageService.setUser(user);
          this.storageService.setToken(token);
          this.user = user;
          console.log(this.user);
          if (this.user.mutedOn != null) {
            this.mutedUntil.setDate(this.user.mutedOn.getDate() + this.user.mutedFor);
            console.log(this.mutedUntil);
          }
        }
        return user;
      })
    );
  }

  getUserByUsername(username: string) {
    return this.http.get<User>(this.accUrl + '/getUser?username=' + username);
  }

  muteUser(username: string, period: number) {
    this.http.post(this.accUrl + '/muteUser?username=' + username + '&period=' + period, "").subscribe(_ => console.log("done"));
  }

  banUser(username: string) {
    this.http.post(this.accUrl + '/banUser?username=' + username, "").subscribe(_ => console.log("done"));
  }
}
