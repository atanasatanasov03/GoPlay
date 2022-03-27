import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
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
  loginForm: FormGroup;

  constructor(
    public userService: UserServiceService,
    public localStorage: LocalStorageService,
    public messageService: MessageServiceService,
    public notificationService: NotificationService,
    private builder: FormBuilder,
    private router: Router
    ) { }

  ngOnInit() {
    this.loginForm = this.builder.group({
      username: ['', Validators.required],
      password: ['', Validators.required]
    });
  }

  login() {
    if(this.loginForm.invalid) {
      if(this.loginForm.get("username").errors?.required && this.loginForm.get("password").errors?.required)
        this.notificationService.showError("Username and Password cannot be empty", "")
      else if(this.loginForm.get("username").errors?.required)
        this.notificationService.showError("Username cannot be empty", "")
      else if(this.loginForm.get("password").errors?.required)
        this.notificationService.showError("Password cannot be empty", "")

      return;
    }
    this.userService.login(this.buildModel()).subscribe(response => {
      if (this.localStorage.getUser().banned) {
        this.router.navigate(["/"])
      }

      this.localStorage.setToken(response)
      this.messageService.connect();
      this.messageService.getGroups();

      this.notificationService.showSuccess("Hello " + this.loginForm.get("username").value, "Successfull login");
      
      this.router.navigate(['/home'])

      this.loginForm.controls.username.setValue('');
      this.loginForm.controls.password.setValue('');
    }, error => {
      this.notificationService.showError("Incorrext username or password", "Bad login credentials")
    })
  }

  buildModel() {
    return {
      username: this.loginForm.get("username").value,
      password: this.loginForm.get("password").value
    }
  }

  sendToHome() {
    this.router.navigate(['/home'])
  }

  sendToMessages() {
    this.router.navigate(['/messages'])
  }

  logout() {
    this.router.navigate([''])
    this.localStorage.setUser(null);
    this.notificationService.showInfo("You have logged out of your account", "Logout")
  }
}
