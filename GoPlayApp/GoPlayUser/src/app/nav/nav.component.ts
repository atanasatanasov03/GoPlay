import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { User } from '../models/User';
import { LocalStorageService } from '../services/local-storage.service';
import { MessageServiceService } from '../services/message-service.service';
import { NotificationService } from '../services/notification.service';
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
    public messageService: MessageServiceService,
    public notificationService: NotificationService,
    private router: Router
    ) { }

  ngOnInit(): void {
    this.logged = this.userService.isLogged()
  }

  login() {
    this.userService.login(this.model).subscribe(response => {
      if (this.userService.isBanned) {
        console.log(this.userService.isBanned)
        this.router.navigate(["/"])
      }

      this.localStorage.setToken(response)
      this.logged = this.userService.isLogged();
      this.messageService.connect();
      this.messageService.getGroups();

      this.notificationService.showSuccess("Hello " + this.model.username, "Successfull login");

      this.model.username = "";
      this.model.password = "";
      
      this.router.navigate(['/home'])
      
    }, error => {
      this.notificationService.showError("Incorrext username or password", "Bad login credentials")
    })
  }

  sendToHome() {
    this.router.navigate(['/home'])
  }

  sendToMessages() {
    this.router.navigate(['/messages'])
  }

  logout() {
    localStorage.removeItem('user');
    this.logged = false;
    this.router.navigate([''])
    this.userService.setCurrentUser(null);
    this.notificationService.showInfo("You have logged out of your account", "Logout")
  }
}
