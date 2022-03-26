import { HttpClient } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { LocalStorageService } from 'app/services/local-storage.service';
import { UserServiceService } from '../services/user.service';

@Component({
  selector: 'app-index',
  templateUrl: './index.component.html',
  styleUrls: ['./index.component.css']
})
export class IndexComponent implements OnInit {
  logged = false;
  registerMode = false;
  users: any;

  constructor(private http: HttpClient,
    public localStorage: LocalStorageService,
    public userService: UserServiceService,
    private router: Router) { }

  ngOnInit(): void {
    if(this.localStorage.getUser() != null) this.router.navigate(['/home'])
  }

  redirect() {
    this.router.navigate(['/registerCenter'])
  }

  registerToggle() {
    this.registerMode = !this.registerMode;
  }

  cancelRegisterMode(event: boolean) {
    this.registerMode = event;
  }

  isLogged() {
    if (this.localStorage.getUser() != null) {
      console.log("logged");
      return true;
    }
    else {
      return false;
    }
  }

}
