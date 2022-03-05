import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { User } from '../models/User';
import { LocalStorageService } from '../services/local-storage.service';
import { UserServiceService } from '../services/user.service';

@Component({
  selector: 'app-nav',
  templateUrl: './nav.component.html',
  styleUrls: ['./nav.component.css']
})
export class NavComponent implements OnInit {
  model: any = {};
  logged: boolean;

  constructor(
    public userService: UserServiceService,
    public localStorage: LocalStorageService,
    private router: Router
    ) { }

  ngOnInit(): void {
    this.logged = this.userService.isLogged()
  }

  login() {
    console.log(this.model)
    this.userService.login(this.model).subscribe(response => {
      this.localStorage.setToken(response)
      this.logged = this.userService.isLogged();
      console.log("in login()", this.logged)

      this.router.navigate(['/home'])
    }, error => {
      console.log(error);
    })
  }

  sendToHome() {
    console.log("sending to home");
    this.router.navigate(['/home'])
  }

  sendToMessages() {
    console.log("sending to messages");
    this.router.navigate(['/messages'])
  }

}
