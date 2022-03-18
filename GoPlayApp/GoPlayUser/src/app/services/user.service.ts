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
  logged: boolean;
  username: string;
  role: string;
  muted: boolean = false;
  isBanned: boolean = false;

  private currentUserSource = new ReplaySubject<User>(1);
  currentUser$ = this.currentUserSource.asObservable();

  constructor(private http: HttpClient, private storageService: LocalStorageService, private router: Router) { }

  register(model: any) {
    return this.http.post(this.accUrl + '/register', model).pipe(
      map((response: any) => {
        const user = response;
        if (user) {
          this.storageService.setUser(user);
          this.setCurrentUser(user);
          console.log(user);

          this.logged = true;
          this.role = user.role;
          this.username = user.username;
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
          this.setCurrentUser(user);
          console.log(user);

          this.role = user.role;
          this.logged = true; 
          this.username = user.userName;
          if (user.mutedOn != null) {
            this.muted = true;
          }
          if (user.banned) {
            this.isBanned = true;
          }
        }
        return user;
      })
    );
  }

  isLogged() {
    return this.logged; 
  }


  setCurrentUser(user: User) {
    this.currentUserSource.next(user);
  }

  getUserByUsername(username: string) {
    return this.http.get<User>(this.accUrl + '/getUser?username=' + username);
  }

  getCurrentUser() {
    let user: User | undefined = JSON.parse(localStorage.getItem('user')!);
    return user == null ? null : user;
  }

  muteUser(username: string, period: number) {
    this.http.post(this.accUrl + '/muteUser?username=' + username + '&period=' + period, "").subscribe(_ => console.log("done"));
  }

  banUser(username: string) {
    this.http.post(this.accUrl + '/banUser?username=' + username, "").subscribe(_ => console.log("done"));
  }
}
