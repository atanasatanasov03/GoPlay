import { HttpClient } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
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
    public accountService: UserServiceService,
    private router: Router) { }

  ngOnInit(): void {
    this.logged = (this.accountService as any).isLogged();
  }

  redirect() {
    this.router.navigate(['/registerCenter'])
  }

  registerToggle() {
    this.registerMode = !this.registerMode;
    console.log(this.registerMode);
  }

  cancelRegisterMode(event: boolean) {
    this.registerMode = event;
  }

  isLogged() {
    if (this.logged == true) {
      console.log("logged");
      return true;
    }
    else {
      return false;
    }
  }

}
